using Application.Repositories;
using Data.Entities.Generated;
using Domain.Shared.DTO.Equipment;
using Domain.Shared.DTO.Equipment.Compatibility;
using Domain.Shared.DTO.Equipment.Model;
using Microsoft.EntityFrameworkCore;


namespace Application.Services.Equipments;

public interface IEquipmentModelService
{
    Task<EquipmentModelDetailDto?> GetEquipmentModelDetialsAsync(int modelId);
    Task<EquipmentModelListItemDto> CreateEquipmentModelAsync(CreateModelDto dto);
    Task<EquipmentModelListItemDto?> UpdateEquipmentModelAsync(int modelId, UpdateModelDto dto);
    Task<List<EquipmentModelListItemDto>> GetEquipmentModelListItemsAsync();
    Task<bool> DeleteEquipmentModelAsync(int modelId);
}

public sealed class EquipmentModelService(
    IBaseRepository<EquipmentModel> equipmentModelRepository,
    IBaseRepository<EquipmentBrand> equipmentBrandRepository,
    IBaseRepository<EquipmentCategory> equipmentCategoryRepository
) : IEquipmentModelService
{
    public async Task<EquipmentModelDetailDto?> GetEquipmentModelDetialsAsync(int modelId)
    {
        var model = await equipmentModelRepository.Query(asNoTracking: true)
            .Where(m => m.Id == modelId)
            .Select(m => new EquipmentModelDetailDto(
                m.Id,
                m.Name,
                m.Slug,
                m.BrandId,
                m.EquipmentBrand.Name,
                m.CategoryId,
                m.EquipmentCategory.Name,
                m.EquipmentCategory.Accessory,
                m.EquipmentCategory.SpecsType,
                m.Url,
                m.Specifications,
                m.FitsTelescop,
                m.FitsInstrume,
                m.Equipments.Count,
                m.CreatedAt,
                m.Compatibilities.Select(c => new CompatibleWithDto(
                    c.CompatibleWithModelId,
                    c.CompatibleWithModel.Name,
                    c.CompatibleWithModel.EquipmentCategory.Name
                )).ToList(),
                m.Equipments.Select(e => new EquipmentListItemDto(
                    e.Id,
                    e.Code,
                    e.SerialNumber,
                    null,
                    null,
                    m.Name,
                    m.EquipmentBrand.Name,
                    m.EquipmentCategory.Name,
                    e.Status,
                    e.ChildParts.Count,
                    e.Location,
                    e.TotalUsageHours,
                    e.CreatedAt
                )).ToList()
            ))
            .FirstOrDefaultAsync();

        return model;
    }

    public async Task<EquipmentModelListItemDto> CreateEquipmentModelAsync(CreateModelDto dto)
    {
        var name = NormalizeRequired(dto.Name, "Model name is required.");
        var slug = NormalizeSlug(dto.Slug, name);

        await EnsureBrandExistsAsync(dto.BrandId);
        await EnsureCategoryExistsAsync(dto.CategoryId);
        await EnsureSlugIsUniqueAsync(slug);

        var entity = new EquipmentModel
        {
            Name = name,
            Slug = slug,
            BrandId = dto.BrandId,
            CategoryId = dto.CategoryId,
            Url = NormalizeOptional(dto.Url),
            Specifications = NormalizeOptional(dto.Specifications),
            FitsTelescop = NormalizeOptional(dto.FitsTelescop),
            FitsInstrume = NormalizeOptional(dto.FitsInstrume),
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        await equipmentModelRepository.AddAsync(entity);

        return await GetListItemAsync(entity.Id)
            ?? throw new InvalidOperationException("Model was created but could not be loaded.");
    }

    public async Task<EquipmentModelListItemDto?> UpdateEquipmentModelAsync(int modelId, UpdateModelDto dto)
    {
        var entity = await equipmentModelRepository.GetByIdAsync(modelId);
        if (entity is null)
        {
            return null;
        }

        var name = NormalizeRequired(dto.Name, "Model name is required.");
        var slug = NormalizeSlug(dto.Slug, name);

        await EnsureCategoryExistsAsync(dto.CategoryId);
        await EnsureSlugIsUniqueAsync(slug, modelId);

        entity.Name = name;
        entity.Slug = slug;
        entity.CategoryId = dto.CategoryId;
        entity.Url = NormalizeOptional(dto.Url);
        entity.Specifications = NormalizeOptional(dto.Specifications);
        entity.FitsTelescop = NormalizeOptional(dto.FitsTelescop);
        entity.FitsInstrume = NormalizeOptional(dto.FitsInstrume);
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        await equipmentModelRepository.UpdateAsync(entity);

        return await GetListItemAsync(modelId);
    }

    public async Task<bool> DeleteEquipmentModelAsync(int modelId)
    {
        var entity = await equipmentModelRepository.Query()
            .Include(m => m.Equipments)
            .Include(m => m.Compatibilities)
            .Include(m => m.CompatibleWith)
            .FirstOrDefaultAsync(m => m.Id == modelId);

        if (entity is null)
        {
            return false;
        }

        if (entity.Equipments.Count != 0)
        {
            throw new InvalidOperationException("Cannot delete a model that has equipment records.");
        }

        if (entity.Compatibilities.Count != 0 || entity.CompatibleWith.Count != 0)
        {
            throw new InvalidOperationException("Cannot delete a model that has compatibility records.");
        }

        await equipmentModelRepository.DeleteAsync(entity);
        return true;
    }

    private async Task<EquipmentModelListItemDto?> GetListItemAsync(int modelId)
    {
        return await equipmentModelRepository.Query(asNoTracking: true)
            .Where(m => m.Id == modelId)
            .Select(m => new EquipmentModelListItemDto(
                m.Id,
                m.Name,
                m.EquipmentBrand.Name,
                m.EquipmentCategory.Name,
                m.Url,
                m.EquipmentCategory.Accessory,
                m.Equipments.Count,
                m.CreatedAt
            ))
            .FirstOrDefaultAsync();
    }
    public async Task<List<EquipmentModelListItemDto>> GetEquipmentModelListItemsAsync()
    {
        // models
        var models = await equipmentModelRepository.Query()
                    .Include(m => m.EquipmentBrand)
                    .Include(m => m.EquipmentCategory)
                    .Select(m => new EquipmentModelListItemDto(
                        m.Id,
                        m.Name,
                        m.EquipmentBrand.Name,
                        m.EquipmentCategory.Name,
                        m.Url,
                        m.EquipmentCategory.Accessory,
                        m.Equipments.Count,
                        m.CreatedAt
                    ))
                    .ToListAsync();

        return models;

    }

    private async System.Threading.Tasks.Task EnsureBrandExistsAsync(int brandId)
    {
        var exists = await equipmentBrandRepository.Query(asNoTracking: true)
            .AnyAsync(b => b.Id == brandId);

        if (!exists)
        {
            throw new InvalidOperationException("Brand not found.");
        }
    }

    private async System.Threading.Tasks.Task EnsureCategoryExistsAsync(int categoryId)
    {
        var exists = await equipmentCategoryRepository.Query(asNoTracking: true)
            .AnyAsync(c => c.Id == categoryId);

        if (!exists)
        {
            throw new InvalidOperationException("Category not found.");
        }
    }

    private async System.Threading.Tasks.Task EnsureSlugIsUniqueAsync(string slug, int? excludingModelId = null)
    {
        var exists = await equipmentModelRepository.Query(asNoTracking: true)
            .AnyAsync(m => m.Slug == slug && (!excludingModelId.HasValue || m.Id != excludingModelId.Value));

        if (exists)
        {
            throw new InvalidOperationException("Model slug already exists.");
        }
    }

    private static string NormalizeRequired(string value, string errorMessage)
    {
        var normalized = value.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new InvalidOperationException(errorMessage);
        }

        return normalized;
    }

    private static string NormalizeSlug(string? slug, string fallback)
    {
        var normalized = string.IsNullOrWhiteSpace(slug) ? fallback : slug;
        normalized = normalized.Trim().ToLowerInvariant().Replace(" ", "-").Replace("_", "-");

        return System.Text.RegularExpressions.Regex.Replace(normalized, @"[^a-z0-9\-]", "");
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
