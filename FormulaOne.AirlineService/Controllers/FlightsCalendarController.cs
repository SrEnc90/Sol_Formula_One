using FormulaOne.AirlineService.Services;
using FormulaOne.Entities.Dtos.Common;
using Microsoft.AspNetCore.Mvc;

namespace FormulaOne.AirlineService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsCalendarController : ControllerBase
{
    private readonly ILogger<FlightsCalendarController> _logger;
    private readonly ICalendarService _calendarService;

    public FlightsCalendarController(
        ILogger<FlightsCalendarController> logger,
        ICalendarService calendarService)
    {
        _logger = logger;
        _calendarService = calendarService;
    }
    
    [HttpGet(Name = "GetAvailableFlights")]
    public async Task<IActionResult> Get()
    {
        try
        {
            var flights =  await _calendarService.GetAvailableFlights();
            return Ok(flights);

        }
        catch (Exception e)
        {
            return StatusCode(500, "Service is not available");
        }
    }
    
}