using FormulaOne.DataService.Data;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FormulaOne.DataService.Repositories;

public class DriverRepository : GenericRepository<Driver>, IDriverRepository
{
    public DriverRepository(ILogger logger, AppDbContext context) : base(logger, context)
    { }

    // con esto estoy sobreescribiendo el método All de genericRpository, pero no sé poque no me coloca el override.
    //Ya pude sobreescribir el método el problema era que dentro de la clase GenericRepository los métodos deben ser vitual's
    public override async Task<ICollection<Driver>?> All()
    {
        try
        {
            return await _dbSet.Where(x => x.Status == 1)
                .AsNoTracking()
                .AsSingleQuery()
                .OrderBy(x => x.AddedDate)
                .ToListAsync();


        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Repo} All function error", typeof(DriverRepository));
            throw;
        }
    }

    public override async Task<bool> Delete(Guid id)
    {
        try
        {
            var driver = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (driver is null) return false;

            driver.Status = 0;
            driver.UpdatedDate=DateTime.UtcNow;
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Repo} Delete function error", typeof(DriverRepository));
            throw;
        }
    }

    public override async Task<bool> Update(Driver entity)
    {
        try
        {
            var driver = await _dbSet.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (driver is null) return false;

            driver.UpdatedDate = DateTime.UtcNow;
            driver.DriverNumber = entity.DriverNumber;
            driver.FirstName = entity.FirstName;
            driver.LastName = entity.LastName;
            driver.DateOfBirth = entity.DateOfBirth;

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Repo} Update function error", typeof(DriverRepository));
            throw;
        }
    }
}