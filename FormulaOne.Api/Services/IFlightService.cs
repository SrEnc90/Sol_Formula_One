using FormulaOne.Entities.Dtos.Common;

namespace FormulaOne.Api.Services;

public interface IFlightService
{
    Task<List<FlightDto>?> GetAllAvailableFlights();
}