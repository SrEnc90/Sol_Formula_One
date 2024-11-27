using FormulaOne.Entities.Dtos.Common;

namespace FormulaOne.AirlineService.Services;

public interface ICalendarService
{
    Task<List<FlightDto>> GetAvailableFlights();
}