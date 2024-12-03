namespace PizzeriaBravo.CustomerService.DataAccess.Interfaces;

public interface IEntity<T> where T : notnull
{
    T Id { get; set; }
}