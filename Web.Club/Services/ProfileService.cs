using Domain.Shared.DTO;
using Web.Club.Providers;

namespace Web.Club.Services;

public interface IProfileService
{
    Task<ProfileDetailsDto?> GetProfileAsync(CancellationToken ct = default);
    Task<ProfileDetailsDto?> UpdateProfileAsync(UpdateProfileRequest request, CancellationToken ct = default);
}

public class ProfileService(ApiHttpClient api) : IProfileService
{
    public Task<ProfileDetailsDto?> GetProfileAsync(CancellationToken ct = default) =>
        api.GetAsync<ProfileDetailsDto>("api/profile", ct);

    public Task<ProfileDetailsDto?> UpdateProfileAsync(UpdateProfileRequest request, CancellationToken ct = default) =>
        api.PutAsync<ProfileDetailsDto>("api/profile", request, ct);
}
