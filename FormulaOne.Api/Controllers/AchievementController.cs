using AutoMapper;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using FormulaOne.Entities.Dtos.Reponses;
using FormulaOne.Entities.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace FormulaOne.Api.Controllers;

public class AchievementController : BaseController
{
    public AchievementController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    [HttpGet]
    [Route("{driverId:Guid}")]
    public async Task<ActionResult> GetDriverAchievements(Guid driverId)
    {
        var driverAchievements = await _unitOfWork.Achievements.GetDriverAchievementAsync(driverId);
        if (driverAchievements is null)
            return NotFound("Achievements not found");

        var result = _mapper.Map<DriverAchievementResponse>(driverAchievements);
        return Ok(result);
    }

    [HttpPost("")]
    public async Task<ActionResult> AddAchievement([FromBody] CreateDriverAchievementRequest achievement)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var result = _mapper.Map<Achievement>(achievement);

        await _unitOfWork.Achievements.Add(result);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetDriverAchievements), new { driverId = result.DriverId }, result);
    }

    [HttpPut("")]
    public async Task<ActionResult> UpdateAchievements([FromBody] UpdateDriverAchievementRequest achievement)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var result = _mapper.Map<Achievement>(achievement);

        await _unitOfWork.Achievements.Update(result);
        await _unitOfWork.CompleteAsync();

        // return CreatedAtAction(nameof(GetDriverAchievements), new { driverId = result.DriverId }, result);
        return NoContent();
    }
}