using FormulaOne.DataService.Data.Configurations;
using FormulaOne.Entities.DbSet;
using Microsoft.EntityFrameworkCore;

namespace FormulaOne.DataService.Data;

public class AppDbContext : DbContext
{
    //Ojo para hacer las migraciones debo colocar el proyecto de inicio:
    //dotnet ef migrations add "Initial_Migration" --startup-project ../FormulaOne.Api/
    // dotnet ef database update --startup-project ../FormulaOne.Api/
    // Se va a crear un app.db en mi proyecto FormulaOne.Api

    //Define the db entities
    public DbSet<Driver> Drivers { get; set; }
    //Averiguar la diferencia de colocar virtual
    // public virtual DbSet<Achievement> Achievements { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //specified the relationship between entities
        // modelBuilder.Entity<Achievement>(entity =>
        // {
        //     entity.HasOne(d => d.Driver)
        //         .WithMany(p => p.Achievements)
        //         .HasForeignKey(d => d.DriverId)
        //         .OnDelete(DeleteBehavior.NoAction)
        //         .HasConstraintName("FK_Achievements_Driver");
        // });

        //para todos las configuraciones
        // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        //uno por uno las configuraciones
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AchievementConfig).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventConfig).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TicketConfig).Assembly);
        
        SeedData(modelBuilder);
        
        base.OnModelCreating(modelBuilder);
    }
    
    private void SeedData(ModelBuilder modelBuilder)
    {
        var demoEvent = new Event()
        {
            Id = 1,
            Name = "British Grand Prix",
            Location = "Silverstone Circuit",
        };
        modelBuilder.Entity<Event>().HasData(demoEvent);
        
        var tickets = Enumerable
            .Range(1, 5000)
            .Select(id => new Ticket()
            {
                Id = Guid.NewGuid(),
                EventDate = DateTime.UtcNow.AddDays(10),
                Price = 100,
                Status = 1,
                EventId = 1,
                AddedDate = DateTime.UtcNow,
                TicketLevel = "Bronze",
                UpdatedDate = DateTime.UtcNow
            });
        modelBuilder.Entity<Ticket>().HasData(tickets);
    }

}