using PizzeriaBravo.CustomerService.API.Interfaces;
using PizzeriaBravo.CustomerService.DataAccess;
using PizzeriaBravo.CustomerService.DataAccess.Repositories;

namespace PizzeriaBravo.CustomerService.API;

public class UnitOfWork(AppDbContext? appDbContext) : IUnitOfWork, IDisposable
{
    private CustomerRepository? _customerRepository;

    public CustomerRepository CustomerRepository
    {
        get
        {
            if (appDbContext is not null)
            {
                return _customerRepository ??= new CustomerRepository(appDbContext);
            }

            throw new ArgumentNullException(nameof(appDbContext));
        }
    }

    public Task SaveChangesAsync()
    {
        if (appDbContext is not null)
        {
            return appDbContext.SaveChangesAsync();
        }

        throw new ArgumentNullException(nameof(appDbContext));
    }

    public void Dispose()
    {
        appDbContext?.Dispose();
    }
}