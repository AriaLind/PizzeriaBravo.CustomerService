namespace PizzeriaBravo.CustomerService.DataAccess.Interfaces;

public interface IRepository<TId, TEntity> where TId : notnull where TEntity : IEntity<TId>
{
    Task<TEntity?> GetByIdAsync(TId id);
    Task<IEnumerable<TEntity?>> GetAllAsync();
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TId id);
}