using Application.Repositories;
using Data.Entities.Generated;
using Domain.Shared.DTO;
using Domain.Shared.DTO.Equipment.Brand;
using Domain.Shared.DTO.Equipment.Model;
using Microsoft.EntityFrameworkCore;


namespace Application.Services.Equipments;

public interface IEquipmentBrandService
{
    Task<PagedResult<EquipmentBrandlListItemDto>> GetAll(PagedQueryParams queryParams);
    Task<EquipmentBrandDetialsDto?> GetEquipmentBrandDetialsAsync(int brandId);
    Task<EquipmentBrandlListItemDto> CreateEquipmentBrandAsync(CreateEquipmentBrandDto dto);
}

public sealed class EquipmentBrandService(
    IBaseRepository<EquipmentBrand> equipmentBrandRepository
) : IEquipmentBrandService
{
    public async Task<PagedResult<EquipmentBrandlListItemDto>> GetAll(PagedQueryParams queryParams)
    {
        var baseQuery = equipmentBrandRepository.Query()
                        .Where(p =>
                            string.IsNullOrEmpty(queryParams.Search) ||
                            p.Name.Contains(queryParams.Search) ||
                            (p.CountryOfOrigin != null && p.CountryOfOrigin.Contains(queryParams.Search))
                        );
        baseQuery = ApplyOrdering(baseQuery, queryParams.OrderBy);
        var totalCount = await baseQuery.CountAsync();
        var data = await baseQuery
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .Select(e => new EquipmentBrandlListItemDto
            (
                e.Id,
                e.Name,
                e.Slug,
                e.CountryOfOrigin,
                e.LogoUrl,
                e.Notes,
                e.EquipmentModels.Count,
                e.CreatedAt
            ))
            .ToListAsync();
        return new PagedResult<EquipmentBrandlListItemDto>(
            data,
            totalCount,
            queryParams.PageNumber,
            queryParams.PageSize
        );
    }

    private static IQueryable<EquipmentBrand> ApplyOrdering(IQueryable<EquipmentBrand> query, string? orderBy)
    {
        return orderBy?.ToUpper() switch
        {
            "NAME ASC" => query.OrderBy(x => x.Name),
            "NAME DESC" => query.OrderByDescending(x => x.Name),

            "COUNTRYOFORIGIN ASC" => query.OrderBy(x => x.CountryOfOrigin),
            "COUNTRYOFORIGIN DESC" => query.OrderByDescending(x => x.CountryOfOrigin),

            "ISACTIVE ASC" => query.OrderBy(x => x.IsActive),
            "ISACTIVE DESC" => query.OrderByDescending(x => x.IsActive),

            "CREATEDAT ASC" => query.OrderBy(x => x.CreatedAt),
            "CREATEDAT DESC" => query.OrderByDescending(x => x.CreatedAt),

            _ => query.OrderByDescending(x => x.CreatedAt)
        };
    }

    public async Task<EquipmentBrandDetialsDto?> GetEquipmentBrandDetialsAsync(int brandId)
    {
        var brand = await equipmentBrandRepository.Query()
            .Include(b => b.EquipmentModels)
                .ThenInclude(m => m.EquipmentCategory)
            .Select(b => new
            {
                b.Id,
                b.Name,
                b.Slug,
                b.CountryOfOrigin,
                b.LogoUrl,
                b.Notes,
                b.IsActive,
                b.CreatedAt,
                Models = b.EquipmentModels.Select(m => new
                {
                    m.Id,
                    m.Name,
                    BrandName = b.Name,
                    CategoryName = m.EquipmentCategory.Name,
                    m.Url,
                    Accessory = m.EquipmentCategory.Accessory,
                    EquipmentsCount = m.Equipments.Count(),   // COUNT(*) in SQL
                    m.CreatedAt
                }).ToList()
            })
            .FirstOrDefaultAsync(b => b.Id == brandId);

        if (brand == null)
            return null;

        var models = brand.Models.Select(m => new EquipmentModelListItemDto(
            m.Id,
            m.Name,
            m.BrandName,
            m.CategoryName,
            m.Url,
            m.Accessory,
            m.EquipmentsCount,
            m.CreatedAt
        )).ToList();

        return new EquipmentBrandDetialsDto(
            brand.Id,
            brand.Name,
            brand.Slug,
            brand.CountryOfOrigin,
            brand.LogoUrl,
            brand.Notes,
            ModelsCount: models.Count,
            AccessoryCount: models.Count(m => m.Accessory),
            EquipmentsCount: models.Sum(m => m.EquipmentsCount),
            IsActive: brand.IsActive,
            Models: models,
            CreatedAt: brand.CreatedAt
        );
    }
    public async Task<EquipmentBrandlListItemDto> CreateEquipmentBrandAsync(CreateEquipmentBrandDto dto)
    {
        Console.WriteLine("########## " + dto);
        var slug = dto.Slug.Trim().ToLowerInvariant();

        var exists = await equipmentBrandRepository.Query()
            .AnyAsync(b => b.Slug == slug);

        if (exists)
            throw new InvalidOperationException("Slug already exists.");

        var entity = new EquipmentBrand
        {
            Name = dto.Name.Trim(),
            Slug = slug,
            CountryOfOrigin = dto.CountryOfOrigin,
            LogoUrl = dto.LogoUrl,
            Notes = dto.Notes,
            CreatedAt = DateTimeOffset.UtcNow,
            IsActive = true
        };

        await equipmentBrandRepository.AddAsync(entity);
        return new EquipmentBrandlListItemDto
            (
                entity.Id,
                entity.Name,
                entity.Slug,
                entity.CountryOfOrigin,
                entity.LogoUrl,
                entity.Notes,
                entity.EquipmentModels.Count,
                entity.CreatedAt
            );
    }
}
