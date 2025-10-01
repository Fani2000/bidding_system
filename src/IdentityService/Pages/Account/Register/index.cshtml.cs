using System.Security.Claims;
using Duende.IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Register;

[SecurityHeaders]
[AllowAnonymous]
public class Index(UserManager<ApplicationUser> userManager) : PageModel
{
    private readonly UserManager<ApplicationUser> userManager = userManager;

    [BindProperty]
    public RegisterViewModel Input { get; set; } = default!;

    [BindProperty]
    public bool RegisterSuccess { get; set; }

    public IActionResult OnGet(string returnUrl)
    {
        Input = new RegisterViewModel { ReturnUrl = returnUrl };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Input.Button != "register")
        {
            return RedirectToPage("~/");
        }

        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = Input.Username,
                Email = Input.Email,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                await userManager.AddClaimsAsync(
                    user,
                    new[] { new Claim(JwtClaimTypes.Name, Input.FullName), }
                );

                RegisterSuccess = true;
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }
}
