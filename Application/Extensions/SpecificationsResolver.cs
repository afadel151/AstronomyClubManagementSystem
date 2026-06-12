// Data.Entities.Json/SpecificationsResolver.cs
using System.Text.Json;
using Data.Entities.Enums;
using Data.Entities.Generated;
using Data.Entities.Json;
namespace Application.Extensions;

public static class SpecificationsResolver
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public static T? Deserialize<T>(string? json) where T : class
    {
        if (string.IsNullOrWhiteSpace(json)) return null;
        return JsonSerializer.Deserialize<T>(json, Options);
    }

    public static string? Serialize<T>(T? obj) where T : class
    {
        if (obj is null) return null;
        return JsonSerializer.Serialize(obj, Options);
    }

    // Call this when you have a model + category and need the typed spec
    public static object? Resolve(string? json, EquipmentCategory category) => category.SpecsType switch
    {
        SpecsTypeEnum.Telescope => Deserialize<TelescopeSpecs>(json),
        SpecsTypeEnum.Mount => Deserialize<MountSpecs>(json),
        SpecsTypeEnum.Camera => Deserialize<CameraSpecs>(json),
        SpecsTypeEnum.Filter => Deserialize<FilterSpecs>(json),
        SpecsTypeEnum.Guider => Deserialize<GuiderSpecs>(json),
        SpecsTypeEnum.Focuser => Deserialize<FocuserSpecs>(json),
        SpecsTypeEnum.ReducerFlattener => Deserialize<ReducerFlattenerSpecs>(json),
        _ => null
    };
}