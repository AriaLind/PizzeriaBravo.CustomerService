using PizzeriaBravo.CustomerService.DataAccess.Entities;
using PizzeriaBravo.CustomerService.DataAccess.Interfaces;

namespace PizzeriaBravo.CustomerService.DataAccess.Repositories;

public class CustomerRepository(AppDbContext? appDbContext) : IRepository<Guid, Customer>
{
    public Task<Customer?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Customer>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Customer entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Customer entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}