namespace FormulaOne.Entities.DbSet;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public IEnumerable<Ticket> Tickets { get; set; } = null!;
}