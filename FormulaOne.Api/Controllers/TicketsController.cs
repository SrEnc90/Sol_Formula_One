using FormulaOne.DataService.Data;
using FormulaOne.Entities.DbSet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FormulaOne.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TicketsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetAllTickets()
    {
        var eventTickets = await _context.Events
            .Include(x => x.Tickets)
            .ToListAsync();

        return Ok(eventTickets);
    }

    [HttpPut("{eventId:int}")]
    public async Task<IActionResult> UpdateTicketPrices([FromRoute] int eventId)
    {
        Event? mainEvent = await _context.Events
            .Include(x => x.Tickets)
            .FirstOrDefaultAsync(x => x.Id == eventId);

        if(mainEvent is null) return NotFound();
        
        //No se debe hacer así tarda en ejecutar 16.33s, porque va haciendo updates de uno en uno
        // foreach (var ticket in mainEvent?.Tickets!)
        // {
        //     ticket.Price *= 1.2;
        //     ticket.UpdatedDate = DateTime.UtcNow;
        // }
        
        mainEvent.Location = mainEvent.Location + $" - at {DateTime.UtcNow.Date.Millisecond}";

        //Se hace una sola update(masivo) y tarda 3s. Para que se guarde el mainEvent.location debo colocar BeginTransactionAsync y CommitTransactionAsync

        await _context.Database.BeginTransactionAsync();
        
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE Tickets SET Price = Price * 1.2, UpdatedDate = {DateTime.UtcNow} WHERE EventId = {eventId}");
        
        mainEvent.Location = mainEvent.Location + $" - at {DateTime.UtcNow.Date.Millisecond}";
        
        await _context.SaveChangesAsync();

        await _context.Database.CommitTransactionAsync();

        return NoContent();
    }
}