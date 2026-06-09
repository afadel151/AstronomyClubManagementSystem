using System.ComponentModel.DataAnnotations;
using Application.Repositories;
using Data.Entities.Identity;
using Domain.Shared.DTO;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public interface IProfileService
{
    Task<ProfileDetailsDto> GetProfileAsync(string userId, CancellationToken ct);
    Task<ProfileDetailsDto> UpdateProfileAsync(string userId, UpdateProfileRequest request, CancellationToken ct);
}

public class ProfileService(
    IBaseRepository<ApplicationUser> userRepository,
    UserManager<ApplicationUser> userManager) : IProfileService
{
    public async Task<ProfileDetailsDto> GetProfileAsync(string userId, CancellationToken ct)
    {
        var user = await userRepository.FirstOrDefaultAsync(
            u => u.Id == userId,
            asNoTracking: true,
            cancellationToken: ct)
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var roles = await userManager.GetRolesAsync(user);
        return ToDto(user, roles);
    }

    public async Task<ProfileDetailsDto> UpdateProfileAsync(
        string userId,
        UpdateProfileRequest request,
        CancellationToken ct)
    {
        Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);

        var user = await userRepository.FirstOrDefaultAsync(
            u => u.Id == userId,
            asNoTracking: false,
            cancellationToken: ct)
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        user.FullName = request.FullName.Trim();
        user.DisplayName = Normalize(request.DisplayName);
        user.PhoneNumber = Normalize(request.PhoneNumber);
        user.BirthYear = request.BirthYear;
        user.AavsoObserverCode = Normalize(request.AavsoObserverCode);
        user.Bio = Normalize(request.Bio);
        user.ProfileImageUrl = Normalize(request.ProfileImageUrl);
        user.Nationality = Normalize(request.Nationality);
        user.City = Normalize(request.City);
        user.UpdatedAt = DateTimeOffset.UtcNow;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new ValidationException(errors);
        }

        var roles = await userManager.GetRolesAsync(user);
        return ToDto(user, roles);
    }

    private static ProfileDetailsDto ToDto(ApplicationUser user, IList<string> roles) => new()
    {
        Id = user.Id,
        MemberCode = user.MemberCode,
        FullName = user.FullName,
        DisplayName = user.DisplayName,
        Email = user.Email ?? string.Empty,
        PhoneNumber = user.PhoneNumber,
        JoinDate = user.JoinDate,
        BirthYear = user.BirthYear,
        MemberStatus = user.MemberStatus.ToString(),
        AavsoObserverCode = user.AavsoObserverCode,
        Bio = user.Bio,
        ProfileImageUrl = user.ProfileImageUrl,
        Nationality = user.Nationality,
        City = user.City,
        LastLoginAt = user.LastLoginAt,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt,
        Roles = roles.ToList()
    };

    private static string? Normalize(string? value)
    {
        var trimmed = value?.Trim();
        return string.IsNullOrWhiteSpace(trimmed) ? null : trimmed;
    }
}
