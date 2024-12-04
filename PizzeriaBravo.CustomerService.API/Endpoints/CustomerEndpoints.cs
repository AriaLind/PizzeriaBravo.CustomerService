using Microsoft.AspNetCore.Mvc;
using PizzeriaBravo.CustomerService.API.Interfaces;
using PizzeriaBravo.CustomerService.API.Payloads;
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

            var response = new Response
            {
                Data = customers,
                Message = string.Empty,
                Success = true
            };

            return Results.Ok(response);
        });

        routes.MapGet("/api/customers/{id}", async (Guid id) =>
        {
            var customer = await concreteUnitOfWork.CustomerRepository.GetByIdAsync(id);

            var response = new Response
            {
                Data = customer,
                Message = string.Empty,
                Success = true
            };

            return customer is null ? Results.NotFound() : Results.Ok(response);
        });

        routes.MapPost("/api/customers", async (Customer customer) =>
        {
            customer.Id = Guid.NewGuid();

            var result = await concreteUnitOfWork.CustomerRepository.AddAsync(customer);

            if (result.Equals(true))
            {
                await concreteUnitOfWork.SaveChangesAsync();
            }

            var response = new Response
            {
                Data = customer,
                Message = "Customer created successfully",
                Success = result
            };

            return result ? Results.Created($"/api/customers/{customer.Id}", response) : Results.BadRequest();
        });

        routes.MapPut("/api/customers/{id}", async (Guid id, Customer customer) =>
        {
            if (id != customer.Id)
            {
                return Results.BadRequest("Customer ID mismatch");
            }

            var result = await concreteUnitOfWork.CustomerRepository.UpdateAsync(customer);

            if (result.Equals(true))
            {
                await concreteUnitOfWork.SaveChangesAsync();
            }

            return result ? Results.Ok(result) : Results.BadRequest();
        });

        routes.MapDelete("/api/customers/{id}", async (Guid id) =>
        {
            var result = await concreteUnitOfWork.CustomerRepository.DeleteAsync(id);

            if (result.Equals(true))
            {
                await concreteUnitOfWork.SaveChangesAsync();
            }

            return result ? Results.Ok() : Results.NotFound();
        });
    }
}