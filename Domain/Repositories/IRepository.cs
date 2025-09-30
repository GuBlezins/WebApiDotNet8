using Domain.Entities;

namespace Domain.Repositories;

public interface IRepository<TEntity> where TEntity : Entity
{
    Task<bool> Create(TEntity entity);
    Task<TEntity?> GetById(Guid id);
    Task<IEnumerable<TEntity>> GetAll();
    Task<bool> Delete(Guid id);
    Task<bool> Update(TEntity entity, Guid id);
    Task<bool> SaveAsync();
}
