using FormulaOne.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormulaOne.DataService.Data.Configurations;

public class AchievementConfig : IEntityTypeConfiguration<Achievement>
{
    public void Configure(EntityTypeBuilder<Achievement> entity)
    {
        entity
            .HasOne(d => d.Driver)
            .WithMany(p => p.Achievements)
            .HasForeignKey(d => d.DriverId)
            .OnDelete(DeleteBehavior.NoAction)
            .HasConstraintName("FK_Achievements_Driver");
        
    }
}