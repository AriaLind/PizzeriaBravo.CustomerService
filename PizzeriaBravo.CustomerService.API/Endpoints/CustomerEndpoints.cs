using Microsoft.AspNetCore.Mvc;
using PizzeriaBravo.CustomerService.API.Interfaces;
using PizzeriaBravo.CustomerService.DataAccess.Entities;
using PizzeriaBravo.CustomerService.DataAccess.Interfaces;

namespace PizzeriaBravo.CustomerService.API.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/customers", async ([FromServices] IUnitOfWork unitOfWork) =>
        {
            if (unitOfWork is not UnitOfWork concreteUnitOfWork)
            {
                return Results.BadRequest("Unit of work is not available");
            }

            var customers = await concreteUnitOfWork.CustomerRepository.GetAllAsync();

            return Results.Ok(customers);
        });

        routes.MapGet("/api/customers/{id}", async ([FromServices] IUnitOfWork unitOfWork, Guid id) =>
        {
            if (unitOfWork is not UnitOfWork concreteUnitOfWork)
            {
                return Results.BadRequest("Unit of work is not available");
            }

            var customer = await concreteUnitOfWork.CustomerRepository.GetByIdAsync(id);

            return customer is null ? Results.NotFound() : Results.Ok(customer);
        });

        routes.MapPost("/api/customers", async ([FromServices] IUnitOfWork unitOfWork, Customer customer) =>
        {
            if (unitOfWork is not UnitOfWork concreteUnitOfWork)
            {
                return Results.BadRequest("Unit of work is not available");
            }

            if (customer.Id == Guid.Empty)
            {
                customer.Id = Guid.NewGuid();
            }

            if (string.IsNullOrWhiteSpace(customer.Email) || string.IsNullOrWhiteSpace(customer.Address))
            {
                return Results.BadRequest("Email and address are required");
            }

            await concreteUnitOfWork.CustomerRepository.AddAsync(customer);

            return Results.Created($"/api/customers/{customer.Id}", customer);
        });

        routes.MapPut("/api/customers/{id}", async ([FromServices] IUnitOfWork unitOfWork, Guid id, Customer customer) =>
        {
            if (unitOfWork is not UnitOfWork concreteUnitOfWork)
            {
                return Results.BadRequest("Unit of work is not available");
            }

            if (id != customer.Id)
            {
                return Results.BadRequest("Customer ID mismatch");
            }

            await concreteUnitOfWork.CustomerRepository.UpdateAsync(customer);
            return Results.Ok(customer);
        });

        routes.MapDelete("/api/customers/{id}", async ([FromServices] IUnitOfWork unitOfWork, Guid id) =>
        {
            if (unitOfWork is not UnitOfWork concreteUnitOfWork)
            {
                return Results.BadRequest("Unit of work is not available");
            }

            await concreteUnitOfWork.CustomerRepository.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}