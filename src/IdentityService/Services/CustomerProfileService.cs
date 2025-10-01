using System.Security.Claims;
using Duende.IdentityModel;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;

public class CustomerProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomerProfileService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        var existingClaims = await _userManager.GetClaimsAsync(user);

        var claims = new List<Claim> { new Claim("username", user.UserName ?? string.Empty), };

        context.IssuedClaims.AddRange(claims);
        var userNameClaim = existingClaims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name);
        context.IssuedClaims.Add(userNameClaim);
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}
