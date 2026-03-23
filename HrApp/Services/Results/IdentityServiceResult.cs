using Microsoft.AspNetCore.Identity;

namespace HrApp.Services.Results
{
    public class IdentityServiceResult: BaseResult
    {
        public IdentityResult? IdentityResult { get; set; }
        public SignInResult? SignInResult { get; set; }
    }
}
