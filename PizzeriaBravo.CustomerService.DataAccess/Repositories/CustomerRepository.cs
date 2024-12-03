using Microsoft.EntityFrameworkCore;
using PizzeriaBravo.CustomerService.DataAccess.Entities;
using PizzeriaBravo.CustomerService.DataAccess.Interfaces;

namespace PizzeriaBravo.CustomerService.DataAccess.Repositories;

public class CustomerRepository(AppDbContext appDbContext) : IRepository<Guid, Customer>
{
    public async Task<Customer?> GetByIdAsync(Guid id)
    { 
        return await appDbContext.Customers.FindAsync(id);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await appDbContext.Customers.ToListAsync();
    }

    public async Task AddAsync(Customer entity)
    {
        await appDbContext.Customers.AddAsync(entity);
    }

    public async Task UpdateAsync(Customer entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var customer = await appDbContext.Customers.FindAsync(entity.Id);

        if (customer is null)
        {
            throw new InvalidOperationException("Customer not found");
        }

        customer.Email = entity.Email;
        customer.Address = entity.Address;

        appDbContext.Customers.Update(customer);
    }

    public async Task DeleteAsync(Guid id)
    {
        var customer = await appDbContext.Customers.FindAsync(id);

        if (customer is null)
        {
            throw new InvalidOperationException("Customer not found");
        }

        appDbContext.Customers.Remove(customer);
    }
}