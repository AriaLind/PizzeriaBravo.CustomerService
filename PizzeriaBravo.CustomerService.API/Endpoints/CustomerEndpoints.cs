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

            if (customer is null)
            {
                response.Message = "Customer not found";
                response.Success = false;
            }

            return customer is null ? Results.NotFound(response) : Results.Ok(response);
        });

        routes.MapPost("/api/customers", async (Customer customer) =>
        {
            customer.Id = Guid.NewGuid();

            var result = true;

            var response = new Response
            {
                Data = customer,
                Message = "Customer created successfully",
                Success = result
            };

            try
            {
                await concreteUnitOfWork.CustomerRepository.AddAsync(customer);
            }
            catch(Exception ex)
            {
                result = false;
                response.Message = ex.Message;
            }

            if (result.Equals(true))
            {
                await concreteUnitOfWork.SaveChangesAsync();
            }

            return result ? Results.Created($"/api/customers/{customer.Id}", response) : Results.BadRequest(response);
        });

        routes.MapPut("/api/customers/{id}", async (Guid id, Customer customer) =>
        {
            var result = true;

            var response = new Response
            {
                Data = customer,
                Message = "Customer updated successfully",
                Success = result
            };

            if (await concreteUnitOfWork.CustomerRepository.GetByIdAsync(id) is null)
            {
                response.Message = "Customer not found";
                response.Success = false;
                return Results.NotFound(response);
            }

            if (id != customer.Id)
            {
                response.Message = "Customer ID mismatch";
                response.Success = false;
                return Results.BadRequest(response);
            }

            try
            {
                await concreteUnitOfWork.CustomerRepository.UpdateAsync(customer);
            }
            catch (Exception ex)
            {
                result = false;
                response.Message = ex.Message;
            }

            if (result.Equals(true))
            {
                await concreteUnitOfWork.SaveChangesAsync();
            }

            return result ? Results.Ok(response) : Results.BadRequest(response);
        });

        routes.MapDelete("/api/customers/{id}", async (Guid id) =>
        {
            var result = true;

            var response = new Response
            {
                Data = null,
                Message = "Customer deleted successfully",
                Success = result
            };

            if (await concreteUnitOfWork.CustomerRepository.GetByIdAsync(id) is null)
            {
                response.Message = "Customer not found";
                response.Success = false;
                return Results.NotFound(response);
            }

            try
            {
                await concreteUnitOfWork.CustomerRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                result = false;
                response.Message = ex.Message;
            }

            if (result.Equals(true))
            {
                await concreteUnitOfWork.SaveChangesAsync();
            }

            return result ? Results.Ok(response) : Results.NotFound(response);
        });
    }
}