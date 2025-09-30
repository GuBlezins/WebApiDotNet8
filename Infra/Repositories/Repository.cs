using Domain.Entities;
using Domain.Repositories;
using Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    private readonly AppDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<bool> Create(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        return await SaveAsync();
    }

    public async Task<TEntity?> GetById(Guid id)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id && e.DeletedAt == null);
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        return await _dbSet.Where(e => e.DeletedAt == null).ToListAsync();
    }

    public async Task<bool> Delete(Guid id)
    {
        var entity = await GetById(id);
        if (entity == null) return false;

        entity.DeletedAt = DateTime.UtcNow;
        _dbSet.Update(entity);

        return await SaveAsync();
    }

    public async Task<bool> Update(TEntity entity, Guid id)
    {
        var existingEntity = await GetById(id);
        if (existingEntity == null) return false;

        _context.Entry(existingEntity).CurrentValues.SetValues(entity);

        return await SaveAsync();
    }

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}