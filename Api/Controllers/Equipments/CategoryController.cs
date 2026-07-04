using Application.Services.Equipments;
using Domain.Shared.DTO;
using Domain.Shared.DTO.Equipment.Category;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Equipments;

[ApiController]
[Route("api/equipments/categories")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class CategoryController(IEquipmentCategoryService equipmentCategoryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<EquipmentCategorylListItemDto>>> GetAll(
        [FromQuery] PagedQueryParams queryParams)
    {
        var categories = await equipmentCategoryService.GetAll(queryParams);
        return Ok(categories);
    }

    [HttpGet("{categoryId:int}")]
    public async Task<ActionResult<EquipmentCategoryDetialsDto>> GetCategory(int categoryId)
    {
        var category = await equipmentCategoryService.GetEquipmentCategoryDetialsAsync(categoryId);
        if (category is null)
        {
            return NotFound();
        }

        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<EquipmentCategorylListItemDto>> CreateCategory(CreateEquipmentCategoryDto dto)
    {
        try
        {
            var category = await equipmentCategoryService.CreateEquipmentCategoryAsync(dto);
            return CreatedAtAction(nameof(GetCategory), new { categoryId = category.Id }, category);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{categoryId:int}")]
    public async Task<ActionResult<EquipmentCategorylListItemDto>> UpdateCategory(
        int categoryId,
        UpdateEquipmentCategoryDto dto)
    {
        try
        {
            var category = await equipmentCategoryService.UpdateEquipmentCategoryAsync(categoryId, dto);
            if (category is null)
            {
                return NotFound();
            }

            return Ok(category);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("{categoryId:int}")]
    public async Task<IActionResult> DeleteCategory(int categoryId)
    {
        try
        {
            var deleted = await equipmentCategoryService.DeleteEquipmentCategoryAsync(categoryId);
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
