using FormulaOne.DataService.Data;
using FormulaOne.DataService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FormulaOne.DataService.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    public readonly ILogger _logger; // se pone público para que se pueda leer desde la clase que está implementado la clase GenericRepository
    private readonly AppDbContext _context;
    internal DbSet<T> _dbSet;

    public GenericRepository(
        ILogger logger,
        AppDbContext context
        )
    {
        _logger = logger;
        _context = context;

        _dbSet = context.Set<T>();
    }
    
    public virtual Task<ICollection<T>?> All()
    {
        throw new NotImplementedException();
    }

    public async Task<T?> GetById(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<bool> Add(T entity)
    {
        await _dbSet.AddAsync(entity);
        return true;
    }

    public virtual Task<bool> Update(T entity)
    {
        throw new NotImplementedException();
    }

    public virtual Task<bool> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}