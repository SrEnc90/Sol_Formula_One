using AutoMapper;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.Entities.DbSet;
using FormulaOne.Entities.Dtos.Reponses;
using FormulaOne.Entities.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace FormulaOne.Api.Controllers;

public class DriverController : BaseController
{
    public DriverController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    [HttpGet]
    public async Task<ActionResult> GetAllDriver()
    {
        var drivers = await _unitOfWork.Drivers.All();

        var result = _mapper.Map<ICollection<GetDriverResponse>>(drivers);
        
        return Ok(result);
    }

    [HttpGet]
    [Route("{driverId:Guid}")]
    public async Task<ActionResult> GetDriver(Guid driverId)
    {
        var driver = await _unitOfWork.Drivers.GetById(driverId);
        if (driver is null) return NotFound();

        var result = _mapper.Map<GetDriverResponse>(driver);
        return Ok(result);
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult> AddDriver([FromBody] CreateDriverRequest driver)
    {
        if (!ModelState.IsValid) return BadRequest();

        var result = _mapper.Map<Driver>(driver);

        await _unitOfWork.Drivers.Add(result);
        await _unitOfWork.CompleteAsync();

        return CreatedAtAction(nameof(GetDriver), new { driverId = result.Id }, result);
    }

    [HttpPut]
    [Route("")]
    public async Task<ActionResult> UpdateDriver([FromBody] UpdateDriverRequest driver)
    {
        if (!ModelState.IsValid) return BadRequest();

        var result = _mapper.Map<Driver>(driver);
        
        await _unitOfWork.Drivers.Update(result);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

    [HttpDelete]
    [Route("{driverId:Guid}")]
    public async Task<ActionResult> DeleteDriver(Guid driverId)
    {
        var driver = await _unitOfWork.Drivers.GetById(driverId);
        if (driver is null) return NotFound();

        await _unitOfWork.Drivers.Delete(driverId);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }
}