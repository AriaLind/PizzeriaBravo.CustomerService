using PizzeriaBravo.CustomerService.DataAccess.Entities;

namespace PizzeriaBravo.CustomerService.DataAccess.Interfaces;

public interface IRepository<TId, TEntity> where TId : notnull where TEntity : IEntity<TId>
{
    Task<TEntity?> GetByIdAsync(TId id);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<bool> AddAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(TId id);
}