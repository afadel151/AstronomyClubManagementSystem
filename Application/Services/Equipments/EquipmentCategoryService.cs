using Application.Repositories;
using Data.Entities.Generated;
using Domain.Shared.DTO;
using Domain.Shared.DTO.Equipment.Category;
using Domain.Shared.DTO.Equipment.Model;
using Microsoft.EntityFrameworkCore;


namespace Application.Services.Equipments;

public interface IEquipmentCategoryService
{
    Task<PagedResult<EquipmentCategorylListItemDto>> GetAll(PagedQueryParams queryParams);
    Task<EquipmentCategoryDetialsDto?> GetEquipmentCategoryDetialsAsync(int categoryId);
    Task<EquipmentCategorylListItemDto> CreateEquipmentCategoryAsync(CreateEquipmentCategoryDto dto);
    Task<EquipmentCategorylListItemDto?> UpdateEquipmentCategoryAsync(int categoryId, UpdateEquipmentCategoryDto dto);
    Task<bool> DeleteEquipmentCategoryAsync(int categoryId);
}

public sealed class EquipmentCategoryService(
    IBaseRepository<EquipmentCategory> equipmentCategoryRepository
) : IEquipmentCategoryService
{
    public async Task<PagedResult<EquipmentCategorylListItemDto>> GetAll(PagedQueryParams queryParams)
    {
        var pageNumber = Math.Max(queryParams.PageNumber, 1);
        var pageSize = Math.Clamp(queryParams.PageSize, 1, 100);

        var baseQuery = equipmentCategoryRepository.Query(asNoTracking: true)
            .Where(c =>
                string.IsNullOrEmpty(queryParams.Search) ||
                c.Name.Contains(queryParams.Search) ||
                (c.Description != null && c.Description.Contains(queryParams.Search)));

        baseQuery = ApplyOrdering(baseQuery, queryParams.OrderBy);

        var totalCount = await baseQuery.CountAsync();
        var data = await baseQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new EquipmentCategorylListItemDto(
                c.Id,
                c.Name,
                c.Description,
                c.Accessory,
                c.SpecsType,
                c.EquipmentModels.Count
            ))
            .ToListAsync();

        return new PagedResult<EquipmentCategorylListItemDto>(
            data,
            totalCount,
            pageNumber,
            pageSize
        );
    }

    public async Task<EquipmentCategoryDetialsDto?> GetEquipmentCategoryDetialsAsync(int categoryId)
    {
        var category = await equipmentCategoryRepository.Query(asNoTracking: true)
            .Where(c => c.Id == categoryId)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.Description,
                c.Accessory,
                c.SpecsType,
                Models = c.EquipmentModels.Select(m => new
                {
                    m.Id,
                    m.Name,
                    BrandName = m.EquipmentBrand.Name,
                    CategoryName = c.Name,
                    m.Url,
                    Accessory = c.Accessory,
                    EquipmentsCount = m.Equipments.Count(),
                    m.CreatedAt
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (category is null)
        {
            return null;
        }

        var models = category.Models.Select(m => new EquipmentModelListItemDto(
            m.Id,
            m.Name,
            m.BrandName,
            m.CategoryName,
            m.Url,
            m.Accessory,
            m.EquipmentsCount,
            m.CreatedAt
        )).ToList();

        return new EquipmentCategoryDetialsDto(
            category.Id,
            category.Name,
            category.Description,
            category.Accessory,
            category.SpecsType,
            models.Count,
            models.Sum(m => m.EquipmentsCount),
            models
        );
    }

    public async Task<EquipmentCategorylListItemDto> CreateEquipmentCategoryAsync(CreateEquipmentCategoryDto dto)
    {
        var name = dto.Name.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidOperationException("Category name is required.");
        }

        await EnsureNameIsUniqueAsync(name);

        var entity = new EquipmentCategory
        {
            Name = name,
            Description = NormalizeOptional(dto.Description),
            Accessory = dto.Accessory,
            SpecsType = dto.SpecsType
        };

        await equipmentCategoryRepository.AddAsync(entity);

        return ToListItem(entity, 0);
    }

    public async Task<EquipmentCategorylListItemDto?> UpdateEquipmentCategoryAsync(
        int categoryId,
        UpdateEquipmentCategoryDto dto)
    {
        var entity = await equipmentCategoryRepository.GetByIdAsync(categoryId);
        if (entity is null)
        {
            return null;
        }

        var name = dto.Name.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidOperationException("Category name is required.");
        }

        await EnsureNameIsUniqueAsync(name, categoryId);

        entity.Name = name;
        entity.Description = NormalizeOptional(dto.Description);
        entity.Accessory = dto.Accessory;
        entity.SpecsType = dto.SpecsType;

        await equipmentCategoryRepository.UpdateAsync(entity);

        var modelsCount = await equipmentCategoryRepository.Query(asNoTracking: true)
            .Where(c => c.Id == categoryId)
            .Select(c => c.EquipmentModels.Count)
            .FirstAsync();

        return ToListItem(entity, modelsCount);
    }

    public async Task<bool> DeleteEquipmentCategoryAsync(int categoryId)
    {
        var entity = await equipmentCategoryRepository.Query()
            .Include(c => c.EquipmentModels)
            .FirstOrDefaultAsync(c => c.Id == categoryId);

        if (entity is null)
        {
            return false;
        }

        if (entity.EquipmentModels.Count != 0)
        {
            throw new InvalidOperationException("Cannot delete a category that has equipment models.");
        }

        await equipmentCategoryRepository.DeleteAsync(entity);
        return true;
    }

    private static IQueryable<EquipmentCategory> ApplyOrdering(
        IQueryable<EquipmentCategory> query,
        string? orderBy)
    {
        return orderBy?.ToUpperInvariant() switch
        {
            "NAME ASC" => query.OrderBy(x => x.Name),
            "NAME DESC" => query.OrderByDescending(x => x.Name),

            "ACCESSORY ASC" => query.OrderBy(x => x.Accessory),
            "ACCESSORY DESC" => query.OrderByDescending(x => x.Accessory),

            "SPECSTYPE ASC" => query.OrderBy(x => x.SpecsType),
            "SPECSTYPE DESC" => query.OrderByDescending(x => x.SpecsType),

            "MODELSCOUNT ASC" => query.OrderBy(x => x.EquipmentModels.Count),
            "MODELSCOUNT DESC" => query.OrderByDescending(x => x.EquipmentModels.Count),

            _ => query.OrderBy(x => x.Name)
        };
    }

    private async System.Threading.Tasks.Task EnsureNameIsUniqueAsync(string name, int? excludingCategoryId = null)
    {
        var exists = await equipmentCategoryRepository.Query(asNoTracking: true)
            .AnyAsync(c => c.Name == name && (!excludingCategoryId.HasValue || c.Id != excludingCategoryId.Value));

        if (exists)
        {
            throw new InvalidOperationException("Category name already exists.");
        }
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static EquipmentCategorylListItemDto ToListItem(EquipmentCategory category, int modelsCount)
    {
        return new EquipmentCategorylListItemDto(
            category.Id,
            category.Name,
            category.Description,
            category.Accessory,
            category.SpecsType,
            modelsCount
        );
    }
}
