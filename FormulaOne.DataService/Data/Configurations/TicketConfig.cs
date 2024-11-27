using FormulaOne.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormulaOne.DataService.Data.Configurations;

public class TicketConfig : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> entity)
    {
        // var tickets = Enumerable
        //     .Range(1, 5000)
        //     .Select(id => new Ticket()
        //     {
        //         Id = Guid.NewGuid(),
        //         EventDate = DateTime.UtcNow.AddDays(10),
        //         Price = 100,
        //         Status = 1,
        //         EventId = 1,
        //         AddedDate = DateTime.UtcNow,
        //         TicketLevel = "Bronze",
        //         UpdatedDate = DateTime.UtcNow
        //     });
        //
        // entity.HasData(tickets);
    }
}