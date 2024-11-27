using FormulaOne.DataService.Data;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FormulaOne.DataService.Repositories;

public class AchievementRepository : GenericRepository<Achievement>, IAchievementRepository
{
    public AchievementRepository(ILogger logger, AppDbContext context) : base(logger, context)
    {
    }

    public async Task<Achievement?> GetDriverAchievementAsync(Guid driverId)
    {
        try
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.DriverId == driverId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Repo} GetDriverAchievement function error", typeof(AchievementRepository));
            throw;
        }
    }
    
    public override async Task<ICollection<Achievement>?> All()
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
            _logger.LogError(e, "{Repo} All function error", typeof(AchievementRepository));
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
            _logger.LogError(e, "{Repo} Delete function error", typeof(AchievementRepository));
            throw;
        }
    }

    public override async Task<bool> Update(Achievement entity)
    {
        try
        {
            var achievement = await _dbSet.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (achievement is null) return false;
            
            achievement.UpdatedDate=DateTime.UtcNow;
            achievement.FasterLap = entity.FasterLap;
            achievement.PolePosition = entity.PolePosition;
            achievement.RaceWins = entity.RaceWins;
            achievement.WorldChampionship = entity.WorldChampionship;

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Repo} Update function error", typeof(AchievementRepository));
            throw;
        }
    }
}