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

    public async Task<bool> AddAsync(Customer entity)
    {
        var result = await appDbContext.Customers.AddAsync(entity);

        return result.State == EntityState.Added;
    }

    public async Task<bool> UpdateAsync(Customer entity)
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

        var result = appDbContext.Customers.Update(customer);

        return result.State == EntityState.Modified;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var customer = await appDbContext.Customers.FindAsync(id);

        if (customer is null)
        {
            throw new InvalidOperationException("Customer not found");
        }

        var result = appDbContext.Customers.Remove(customer);
        return result.State == EntityState.Deleted;
    }
}