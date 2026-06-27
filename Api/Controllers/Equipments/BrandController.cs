using Application.Services.Equipments;
using Domain.Shared.DTO;
using Domain.Shared.DTO.Equipment.Brand;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Equipments;

[ApiController]
[Route("api/equipments/brands")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class EquipmentBrandController(IEquipmentBrandService equipmentBrandService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<PagedResult<EquipmentBrandlListItemDto>>> GetAll([FromQuery] PagedQueryParams queryParams)
    {
        var brands = await equipmentBrandService.GetAll(queryParams);
        return Ok(brands);
    }

    [HttpGet("{BrandId}")]
    public async Task<ActionResult<EquipmentBrandDetialsDto>> GetBrand(int BrandId)
    {
        var brand = await equipmentBrandService.GetEquipmentBrandDetialsAsync(BrandId);
        if (brand == null)
        {
            return NotFound();
        }
        return Ok(brand);
    }

    [HttpPost]
    public async Task<ActionResult<EquipmentBrandlListItemDto>> CreateBrand(CreateEquipmentBrandDto dto)
    {
        Console.WriteLine("########## "+ dto);
        var brand = await equipmentBrandService.CreateEquipmentBrandAsync(dto);
        if (brand == null)
        {
            return Problem("Brand not created");
        }
        return Ok(brand);
    }
}