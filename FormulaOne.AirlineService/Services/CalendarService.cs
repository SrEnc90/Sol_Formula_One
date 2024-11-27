using FormulaOne.Entities.Dtos.Common;

namespace FormulaOne.AirlineService.Services;

public class CalendarService : ICalendarService
{
    private DateTime _recoveryTime = DateTime.UtcNow;
    private static readonly Random Random = new();
    
    public Task<List<FlightDto>> GetAvailableFlights()
    {
        if (_recoveryTime > DateTime.UtcNow)
        {
            Console.WriteLine("Service is not available, recovering until " + _recoveryTime);
            throw new Exception("Service is not available");
        }

        if (_recoveryTime < DateTime.UtcNow && Random.Next(1, 2) == 1)
        {
            _recoveryTime = DateTime.UtcNow.AddSeconds(25);
        }

        var flights = new List<FlightDto>()
        {
            new()
            {
                Arrival = "London",
                Departure = "Dubai",
                Price = 1000,
                FlightDate = DateTime.UtcNow.AddDays(1)
            },
            new()
            {
                Arrival = "Paris",
                Departure = "New York",
                Price = 1500,
                FlightDate = DateTime.UtcNow.AddDays(2)
            },
            new()
            {
                Arrival = "Berlin",
                Departure = "Tokyo",
                Price = 2000,
                FlightDate = DateTime.UtcNow.AddDays(3)
            },
            new()
            {
                Arrival = "Madrid",
                Departure = "Moscow",
                Price = 2500,
                FlightDate = DateTime.UtcNow.AddDays(4)
            },
            new()
            {
                Arrival = "Rome",
                Departure = "Beijing",
                Price = 3000,
                FlightDate = DateTime.UtcNow.AddDays(5)
            }
        };
        return Task.FromResult(flights);
    }
}