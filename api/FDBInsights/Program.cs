using FastEndpoints;
using FDBInsights.Data;
using FDBInsights.Repositories;
using FDBInsights.Repositories.Implementation;
using FDBInsights.Service;
using FDBInsights.Service.Implementation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
    )
);

// Add FastEndpoints
builder.Services.AddFastEndpoints();

// Register application services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

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

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseFastEndpoints(c => { c.Endpoints.RoutePrefix = "api"; });

// Keep controllers for backward compatibility
app.MapControllers();

app.Run();