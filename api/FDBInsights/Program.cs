using Asp.Versioning;
using FDBInsights.Common.Helper;
using FDBInsights.Data;
using FDBInsights.Models;
using FDBInsights.Repositories;
using FDBInsights.Repositories.Implementation;
using FDBInsights.Service;
using FDBInsights.Service.Implementation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        connectionString,
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
    )
);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(option =>
{
    option.AssumeDefaultVersionWhenUnspecified =
        true;
    option.DefaultApiVersion = new ApiVersion(1, 0);
    option.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.WithOrigins("http://localhost:5173", "https://reports.film.gov.np",
                    "https://insigths.nepalidev.com.np")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FDBInsights V1", Version = "v1" });
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "FDBInsights V2", Version = "v2" });
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "Using the Authorization header with Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securitySchema);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securitySchema, ["Bearer"] }
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Add FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
// Register application services
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddLogging();
//builder.Services.AddTransient<DatabaseService>();
builder.Services.AddSingleton<IDapperHelper>(c => new DapperHelper(connectionString));
//builder.Services.AddScoped<BaseEndpointCore>();
builder.Services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
builder.Services.AddScoped<IGenericRepository<UserRole>, GenericRepository<UserRole>>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IJwtRepository, JwtRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<ITheaterRepository, TheaterRepository>();
builder.Services.AddScoped<ITheaterService, TheaterService>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddSingleton<DashboardHub>();
builder.Services.AddSingleton<SubscribeProductTableDependency>();


// Register the multi-table change tracking service as a hosted service
builder.Services.AddSingleton<string>(connectionString); // Register connection string
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // app.UseSwaggerUI();
    // app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "My API V2");
    });
}

app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

// Keep controllers for backward compatibility
app.MapControllers();
app.MapHub<DashboardHub>("/dataHub");

// Register the TableDependency service but catch any errors to prevent app crash
try
{
    app.UseSqlTableDependency<SubscribeProductTableDependency>(connectionString);
    Console.WriteLine("SQL Table Dependency configured successfully");
}
catch (Exception ex)
{
    Console.WriteLine($"Error configuring SQL Table Dependency: {ex.Message}");
    // Allow application to continue even if table dependency setup fails
}

app.Run();