using Application.Services;
using Data.Entities.Generated;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Equipments;

[ApiController]
[Route("api/equipments")]
public sealed class EquipmentController(IEquipmentService equipmentService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<List<Equipment>>> GetAll()
    {
        var equipments = await equipmentService.GetAll();
        return Ok(equipments);
    }
}