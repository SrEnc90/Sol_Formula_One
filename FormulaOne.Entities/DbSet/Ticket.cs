namespace FormulaOne.Entities.DbSet;

public class Ticket : BaseEntity
{
    public int EventId { get; set; }
    public DateTime EventDate { get; set; }
    public double Price { get; set; }
    public string TicketLevel { get; set; } = string.Empty;
}