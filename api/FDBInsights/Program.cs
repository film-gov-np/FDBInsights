using FastEndpoints;
using FDBInsights.Common;
using FDBInsights.Common.Filters;
using FDBInsights.Data;
using FDBInsights.Models;
using FDBInsights.Repositories;
using FDBInsights.Repositories.Implementation;
using FDBInsights.Service;
using FDBInsights.Service.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
    )
);
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        // Options to tell the app to read jwt from the cookie
        // by default it will look into the authorization header
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Read the token from the cookie
                var token = context.Request.Cookies["accessToken"];
                if (!string.IsNullOrEmpty(token)) context.Token = token;

                return Task.CompletedTask;
            }
        };
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
// Add FastEndpoints
builder.Services
    .AddFastEndpoints()
    .AddResponseCaching();
// Register application services
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddLogging();
builder.Services.AddScoped<BaseEndpointCore>();
builder.Services.AddScoped<AuthorizedUserFilter>();
builder.Services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
builder.Services.AddScoped<IGenericRepository<UserRole>, GenericRepository<UserRole>>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IJwtRepository, JwtRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Add controllers for backward compatibility (can be removed if fully migrating to FastEndpoints)
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
}

app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseResponseCaching()
    .UseFastEndpoints(c =>
    {
        c.Endpoints.RoutePrefix = "api/v1";
        //c.Endpoints.Filters.Add<AuthorizedUserFilter>();
    });


// Keep controllers for backward compatibility
app.MapControllers();

app.Run();