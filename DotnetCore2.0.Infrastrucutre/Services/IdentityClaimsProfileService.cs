using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DotnetCore2.Infrastrucutre.Constants;
using DotnetCore2.Infrastrucutre.Data.Identity;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace DotnetCore2.Infrastrucutre.Services
{
    public class IdentityClaimsProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsFactory;
        private readonly UserManager<AppUser> _userManager;

        public IdentityClaimsProfileService(UserManager<AppUser> userManager, IUserClaimsPrincipalFactory<AppUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.Name));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName));
            claims.Add(new Claim(IdentityServerConstants.StandardScopes.Email, user.Email));
            // note: to dynamically add roles (ie. for users other than consumers - simply look them up by sub id
            claims.Add(new Claim(ClaimTypes.Role, Roles.Consumer)); // need this for role-based authorization - https://stackoverflow.com/questions/40844310/role-based-authorization-with-identityserver4

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var subject = context.Subject;

            var user = await _userManager.FindByIdAsync(sub);

            //context.IsActive = await ValidateSecurityStamp(user, subject);
            context.IsActive = await ValidateEmailConfirm(user, subject);
            //context.IsActive = user != null;
        }

        private async Task<bool> ValidateSecurityStamp(AppUser user, ClaimsPrincipal subject)
        {
            if (user == null)
                return false;

            if (!_userManager.SupportsUserSecurityStamp)
                return true;

            var securityStamp = subject.Claims.Where(c => c.Type == "security_stamp").Select(c => c.Value).SingleOrDefault();
            if (securityStamp == null)
                return true;

            var dbSecurityStamp = await _userManager.GetSecurityStampAsync(user);
            return dbSecurityStamp == securityStamp;
        }

        private async Task<bool> ValidateEmailConfirm(AppUser user, ClaimsPrincipal subject)
        {
            if (user == null)
                return false;
            var emailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            return emailConfirmed;
            //if (!_userManager.SupportsUserSecurityStamp)
            //    return true;

            //var securityStamp = subject.Claims.Where(c => c.Type == "security_stamp").Select(c => c.Value).SingleOrDefault();
            //if (securityStamp == null)
            //    return true;

            //var dbSecurityStamp = await _userManager.GetSecurityStampAsync(user);
            //return dbSecurityStamp == securityStamp;
        }
    }
}
