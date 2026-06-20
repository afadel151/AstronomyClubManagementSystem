using Application.Repositories;
using Data.Entities.Generated;
using Domain.Shared.DTO;
using Domain.Shared.DTO.Equipment;
using Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;


namespace Application.Services.Equipments;

public interface IEquipmentService
{
    Task<PagedResult<EquipmentListItemDto>> GetAll(PagedQueryParams queryParams);
}

public sealed class EquipmentService(
    IBaseRepository<Equipment> equipmentRepository,
    IStorageService storageService
// IBaseRepository<EquipmentBrand> equipmentBrandRepository,
// IBaseRepository<EquipmentModel> equipmentModelRepository,
// IBaseRepository<EquipmentCategory> equipmentCaregoryRepository,
// IBaseRepository<EquipmentUpload> equipmentUploadRepository,
// IBaseRepository<EquipmentMaintenance> equipmentMaintenanceRepository,
// IBaseRepository<EquipmentModelCompatibility> equipmentModelCompatibility
) : IEquipmentService
{
    public async Task<PagedResult<EquipmentListItemDto>> GetAll(PagedQueryParams queryParams)
    {
        var baseQuery = equipmentRepository.Query()
            .Where(p =>
                string.IsNullOrEmpty(queryParams.Search) ||
                EF.Functions.Like(p.Code, $"%{queryParams.Search}%") ||
                EF.Functions.Like(p.EquipmentModel.Name, $"%{queryParams.Search}%") ||
                EF.Functions.Like(p.EquipmentModel.EquipmentBrand.Name, $"%{queryParams.Search}%") ||
                (p.SerialNumber != null && EF.Functions.Like(p.SerialNumber, $"%{queryParams.Search}%"))
            );
        baseQuery = ApplyOrdering(baseQuery, queryParams.OrderBy);
        var totalCount = await baseQuery.CountAsync();
        var data = await baseQuery
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .Select(e => new
            {
                e.Id,
                e.Code,
                e.SerialNumber,
                MainImageKey = e.EquipmentUploads
                    .Where(u => u.IsMainImage)
                    .Select(u => u.ObjectKey)
                    .FirstOrDefault(),
                ModelName = e.EquipmentModel.Name,
                BrandName = e.EquipmentModel.EquipmentBrand.Name,
                CategoryName = e.EquipmentModel.EquipmentCategory.Name,
                e.Status,
                ChildPartsCount = e.ChildParts.Count,
                e.Location,
                e.TotalUsageHours,
                e.CreatedAt
            })
            .ToListAsync();

        var result = await System.Threading.Tasks.Task.WhenAll(data.Select(async e =>
        {
            var url = e.MainImageKey == null
                ? null
                : await storageService.GetPresignedUrlAsync("equipments", e.MainImageKey);

            return new EquipmentListItemDto(
                e.Id,
                e.Code,
                e.SerialNumber,
                url,
                e.MainImageKey,
                e.ModelName,
                e.BrandName,
                e.CategoryName,
                e.Status,
                e.ChildPartsCount,
                e.Location,
                e.TotalUsageHours,
                e.CreatedAt
            );
        }));

        return new PagedResult<EquipmentListItemDto>(
            [.. result],
            totalCount,
            queryParams.PageNumber,
            queryParams.PageSize
        );
    }

    private static IQueryable<Equipment> ApplyOrdering(
    IQueryable<Equipment> query,
    string? orderBy)
    {
        return orderBy?.ToUpper() switch
        {
            "CODE ASC" => query.OrderBy(x => x.Code),
            "CODE DESC" => query.OrderByDescending(x => x.Code),

            "CREATEDAT ASC" => query.OrderBy(x => x.CreatedAt),
            "CREATEDAT DESC" => query.OrderByDescending(x => x.CreatedAt),

            "TOTALUSAGEHOURS ASC" => query.OrderBy(x => x.TotalUsageHours),
            "TOTALUSAGEHOURS DESC" => query.OrderByDescending(x => x.TotalUsageHours),

            "MODELNAME ASC" => query.OrderBy(x => x.EquipmentModel.Name),
            "MODELNAME DESC" => query.OrderByDescending(x => x.EquipmentModel.Name),

            "BRANDNAME ASC" => query.OrderBy(x => x.EquipmentModel.EquipmentBrand.Name),
            "BRANDNAME DESC" => query.OrderByDescending(x => x.EquipmentModel.EquipmentBrand.Name),

            "CATEGORYNAME ASC" => query.OrderBy(x => x.EquipmentModel.EquipmentCategory.Name),
            "CATEGORYNAME DESC" => query.OrderByDescending(x => x.EquipmentModel.EquipmentCategory.Name),

            _ => query.OrderByDescending(x => x.CreatedAt)
        };
    }
}
