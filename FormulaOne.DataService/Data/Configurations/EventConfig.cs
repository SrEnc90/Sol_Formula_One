using FormulaOne.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormulaOne.DataService.Data.Configurations;

public class EventConfig : IEntityTypeConfiguration<Event>
{
    //Estamos creando la conexión entre event y ticket
    public void Configure(EntityTypeBuilder<Event> entity)
    {
        entity.HasMany(x => x.Tickets)
            .WithOne() // indicamos que la relación es de uno a muchos(un evento tiene muchos tickets)
            .HasForeignKey(t => t.EventId) // indicamos que la clave foránea es EventId
            .IsRequired();

        // var demoEvent = new Event()
        // {
        //     Id = 1,
        //     Name = "British Grand Prix",
        //     Location = "Silverstone Circuit",
        // };
        //
        // entity.HasData(demoEvent);
    }
}