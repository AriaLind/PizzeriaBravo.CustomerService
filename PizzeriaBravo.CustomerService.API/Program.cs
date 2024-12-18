using Microsoft.EntityFrameworkCore;
using PizzeriaBravo.CustomerService.API;
using PizzeriaBravo.CustomerService.API.Endpoints;
using PizzeriaBravo.CustomerService.API.Interfaces;
using PizzeriaBravo.CustomerService.DataAccess;
using Scalar.AspNetCore;

Thread.Sleep(TimeSpan.FromSeconds(15));

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();


builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (!builder.Environment.IsDevelopment())
        {
            var password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
            connectionString = string.Format(connectionString, password);
        }
        options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null
            ));

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