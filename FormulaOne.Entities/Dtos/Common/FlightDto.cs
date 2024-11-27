namespace FormulaOne.Entities.Dtos.Common;

public class FlightDto
{
    public string Departure { get; set; } = string.Empty;
    public string Arrival { get; set; } = string.Empty;
    public DateTime FlightDate { get; set; }
    public int Price { get; set; }
    
}