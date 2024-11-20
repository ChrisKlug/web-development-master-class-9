using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace WebDevMasterClass.Web.Controllers;

[Route("[controller]")]
public class AuthController : Controller
{
    [HttpGet("login")]
    public IActionResult LogIn(string returnUrl = "/")
    {
        if (!Url.IsLocalUrl(returnUrl))
            returnUrl = "/";

        return Challenge(new AuthenticationProperties { RedirectUri = returnUrl });
    }

    [HttpGet("logout")]
    public async Task LogOut()
    {
        await HttpContext.SignOutAsync();
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
    }
}
