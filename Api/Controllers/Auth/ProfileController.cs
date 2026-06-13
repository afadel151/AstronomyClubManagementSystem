using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Application.Services;
using Domain.Shared.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Auth;

[ApiController]
[Route("api/profile")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class ProfileController(IProfileService profileService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ProfileDetailsDto>> Get(CancellationToken ct)
    {
        return Ok(await profileService.GetProfileAsync(GetUserId(), ct));
    }

    [HttpPut]
    public async Task<ActionResult<ProfileDetailsDto>> Update(UpdateProfileRequest request, CancellationToken ct)
    {
        try
        {
            return Ok(await profileService.UpdateProfileAsync(GetUserId(), request, ct));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private string GetUserId() =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("User is not authenticated.");
}
