using Microsoft.EntityFrameworkCore;
using PizzeriaBravo.CustomerService.API;
using PizzeriaBravo.CustomerService.API.Endpoints;
using PizzeriaBravo.CustomerService.API.Interfaces;
using PizzeriaBravo.CustomerService.DataAccess;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Add services to the container.
//builder.Services.AddDbContext<AppDbContext>(
//    options =>
//    {
//        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//    });

builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (!builder.Environment.IsDevelopment())
        {
            var password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
            connectionString = string.Format(connectionString, password);
        }
        options.UseSqlServer(connectionString);

    });

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var unitOfWork = app.Services.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>();
app.MapCustomerEndpoints(unitOfWork);


if (app.Environment.IsDevelopment())
{

}
app.MapOpenApi();
app.MapScalarApiReference();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (db.Database.GetPendingMigrations().Any())
    {
        db.Database.Migrate();
    }
}

app.Run();