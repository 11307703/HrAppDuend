using HrApp.Services.Interfaces;
using HrApp.Services.Results;
using HrApp.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace HrApp.Services
{
    public class IdentityService : IIdentityService
    {
        SignInManager<IdentityUser> _signInManager;
        UserManager<IdentityUser> _userManager;
        public IdentityService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
           _userManager = userManager;
        }
        public async Task<IdentityServiceResult> LoginAsync(string username, string email, string password)
        {
            var result = new IdentityServiceResult();
            try
            {
                if(string.IsNullOrEmpty(username) && string.IsNullOrEmpty(email))
                {
                    result.Failed("Username of email invullen");
                }
                else
                {
                    if (string.IsNullOrEmpty(username))
                    {
                        var user = await _userManager.FindByEmailAsync(email);
                        result.SignInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                    }
                    else
                    {
                        var user = await _userManager.FindByNameAsync(username);
                        result.SignInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Failed(ex.Message);
            }
            return result;
        }

        public async Task<IdentityServiceResult> RegisterAsync(RegisterViewModel registerData)
        {
            var result = new IdentityServiceResult();
            try
            {
                var identityUser = new IdentityUser
                {
                    Email = registerData.Email,
                    UserName = registerData.UserName
                };
                 result.IdentityResult = await _userManager.CreateAsync(identityUser, registerData.Password);

            }
            catch (Exception ex)
            {
                result.Failed(ex.Message);
            }
            return result;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
