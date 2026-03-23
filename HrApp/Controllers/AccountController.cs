using HrApp.Services.Interfaces;
using HrApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HrApp.Controllers
{
    public class AccountController : Controller
    {

        private readonly IIdentityService _service;
        SignInManager<IdentityUser> _signInManager;
        UserManager<IdentityUser> _userManager;

        public AccountController(IIdentityService service, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _service = service;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        #region Login
        public IActionResult Login()
        {
            return View();
        }
        #endregion

        #region Login Username

        [HttpGet]
        public IActionResult LoginUserName()
        {
            return View();
        }

        //TODO
        [HttpPost]
        public async Task<IActionResult> LoginUserName(LoginUserNameViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.LoginAsync("", model.UserName, model.Password);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.ErrorString);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        #endregion

        #region Login Email

        [HttpGet]
        public IActionResult LoginEmail()
        {
            return View();
        }

        //TODO
        [HttpPost]
        public async Task<IActionResult> LoginEmail(LoginEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.LoginAsync("", model.Email, model.Password);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.ErrorString);
                }
                else
                {
                    return RedirectToAction("Index","Home");
                }
            }
            return View(model);
        }
        #endregion

        #region ExternalLogin
        public IActionResult LoginExternalProvider()
        {
            string? redirectUrl = Url.Action("ExternalProviderResponse", "Account");
            string scheme = "oidc";
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(
                    scheme, redirectUrl);
            return new ChallengeResult(scheme, properties);
        }


        public async Task<IActionResult> ExternalProviderResponse()
        {
            ExternalLoginInfo? externalLoginInfo =
                await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                return RedirectToAction(nameof(Login));
            }
            else
            {
                var user = await _userManager.FindByLoginAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey);
                if (user == null)
                {
                    user = await CreateIdentityUserFromClaims(externalLoginInfo);
                }
                await _signInManager.SignInAsync(user, true);
            }
            return RedirectToAction("Index", "Home");
        }

        private async Task<IdentityUser?> CreateIdentityUserFromClaims(ExternalLoginInfo externalLoginInfo)
        {
            Claim? emailClaim = null;

            var claims = externalLoginInfo.Principal.Claims;
            foreach (var c in claims)
            {
                if (c.Type.Contains("email"))
                {
                    emailClaim = c;
                    break;
                }
            }

            //var claim = externalLoginInfo.Principal.FindFirst("email");
            if (emailClaim != null)
            {
                var email = emailClaim.Value;
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new IdentityUser { UserName = email, Email = email };
                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        return null;
                    }
                }
                var loginResult = await _userManager.AddLoginAsync(user, externalLoginInfo);
                if (loginResult.Succeeded)
                {
                    return user;
                }
            }
            return null;
        }
        #endregion


        #region Register

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.RegisterAsync(registerModel);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.ErrorString);
                }
                else
                {
                    return View("Login");
                }
            }
            return View();
        }

        #endregion

        #region Logout

        public async Task<IActionResult> LogoutAsync()
        {
            await _service.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        #endregion
    }
}
