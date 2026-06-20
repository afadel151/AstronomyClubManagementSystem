using Application.Services.Equipments;
using Domain.Shared.DTO;
using Domain.Shared.DTO.Equipment;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Equipments;
[ApiController]
[Route("api/equipments")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class EquipmentController(IEquipmentService equipmentService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<PagedResult<EquipmentListItemDto>>> GetAll([FromQuery] PagedQueryParams queryParams)
    {
        var equipments = await equipmentService.GetAll(queryParams);
        return Ok(equipments);
    }
}