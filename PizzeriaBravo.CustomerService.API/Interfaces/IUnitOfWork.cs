namespace PizzeriaBravo.CustomerService.API.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}