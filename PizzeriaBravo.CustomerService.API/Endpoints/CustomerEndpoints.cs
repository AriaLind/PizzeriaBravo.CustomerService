using Microsoft.AspNetCore.Mvc;
using PizzeriaBravo.CustomerService.API.Interfaces;
using PizzeriaBravo.CustomerService.DataAccess.Entities;
using PizzeriaBravo.CustomerService.DataAccess.Interfaces;

namespace PizzeriaBravo.CustomerService.API.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder routes, [FromServices] IUnitOfWork unitOfWork)
    {
        if (unitOfWork is not UnitOfWork concreteUnitOfWork)
        {
            throw new ArgumentNullException(nameof(unitOfWork));
        }

        routes.MapGet("/api/customers", async () =>
        {
            var customers = await concreteUnitOfWork.CustomerRepository.GetAllAsync();

            return Results.Ok(customers);
        });

        routes.MapGet("/api/customers/{id}", async (Guid id) =>
        {
            var customer = await concreteUnitOfWork.CustomerRepository.GetByIdAsync(id);

            return customer is null ? Results.NotFound() : Results.Ok(customer);
        });

        routes.MapPost("/api/customers", async (Customer customer) =>
        {
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

        routes.MapPut("/api/customers/{id}", async (Guid id, Customer customer) =>
        {
            if (id != customer.Id)
            {
                return Results.BadRequest("Customer ID mismatch");
            }

            await concreteUnitOfWork.CustomerRepository.UpdateAsync(customer);
            return Results.Ok(customer);
        });

        routes.MapDelete("/api/customers/{id}", async (Guid id) =>
        {
            await concreteUnitOfWork.CustomerRepository.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}