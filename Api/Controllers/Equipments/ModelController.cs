using Application.Services.Equipments;
using Domain.Shared.DTO.Equipment.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Equipments;

[ApiController]
[Route("api/equipments/models")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class ModelController(IEquipmentModelService equipmentModelService) : ControllerBase
{
    [HttpGet("{modelId:int}")]
    public async Task<ActionResult<EquipmentModelDetailDto>> GetModel(int modelId)
    {
        var model = await equipmentModelService.GetEquipmentModelDetialsAsync(modelId);
        if (model is null)
        {
            return NotFound();
        }

        return Ok(model);
    }

    [HttpPost]
    public async Task<ActionResult<EquipmentModelListItemDto>> CreateModel(CreateModelDto dto)
    {
        try
        {
            var model = await equipmentModelService.CreateEquipmentModelAsync(dto);
            return CreatedAtAction(nameof(GetModel), new { modelId = model.Id }, model);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{modelId:int}")]
    public async Task<ActionResult<EquipmentModelListItemDto>> UpdateModel(int modelId, UpdateModelDto dto)
    {
        try
        {
            var model = await equipmentModelService.UpdateEquipmentModelAsync(modelId, dto);
            if (model is null)
            {
                return NotFound();
            }

            return Ok(model);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("{modelId:int}")]
    public async Task<IActionResult> DeleteModel(int modelId)
    {
        try
        {
            var deleted = await equipmentModelService.DeleteEquipmentModelAsync(modelId);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}
