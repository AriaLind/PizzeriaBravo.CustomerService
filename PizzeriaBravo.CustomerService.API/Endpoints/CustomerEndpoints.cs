using PizzeriaBravo.CustomerService.API.Interfaces;
using PizzeriaBravo.CustomerService.DataAccess.Entities;
using PizzeriaBravo.CustomerService.DataAccess.Interfaces;

namespace PizzeriaBravo.CustomerService.API.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder routes)
    {
        var unitOfWork = routes.ServiceProvider.GetRequiredService<IUnitOfWork>() as UnitOfWork;

        routes.MapGet("/api/customers", async () =>
        {
            if (unitOfWork is null)
            {
                return Results.BadRequest("Unit of work is not available");
            }

            var customers = await unitOfWork.CustomerRepository.GetAllAsync();

            return Results.Ok(customers);
        });

        routes.MapGet("/api/customers/{id}", async (Guid id) =>
        {
            if (unitOfWork is null)
            {
                return Results.BadRequest("Unit of work is not available");
            }

            var customer = await unitOfWork.CustomerRepository.GetByIdAsync(id);

            return customer is null ? Results.NotFound() : Results.Ok(customer);
        });

        routes.MapPost("/api/customers", async (Customer customer) =>
        {
            if (unitOfWork is null)
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

            await unitOfWork.CustomerRepository.AddAsync(customer);

            return Results.Created($"/api/customers/{customer.Id}", customer);
        });

        routes.MapPut("/api/customers/{id}", async (IRepository<Guid, Customer> repository, Guid id, Customer customer) =>
        {
            if (id != customer.Id)
            {
                return Results.BadRequest("Customer ID mismatch");
            }
            await repository.UpdateAsync(customer);
            return Results.Ok(customer);
        });

        routes.MapDelete("/api/customers/{id}", async (IRepository<Guid, Customer> repository, Guid id) =>
        {
            await repository.DeleteAsync(id);
            return Results.NoContent();
        });
    }

    public static void CheckCorrectFormat()
    {

    }
}