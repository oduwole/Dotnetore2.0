using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DotnetCore2.AuthServer.Repository;

namespace DotnetCore2.AuthServer.Extensions
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        IAuthRepository _rep;

        public ResourceOwnerPasswordValidator(IAuthRepository rep)
        {
            this._rep = rep;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (_rep.EmailConfirmed(context.UserName))
            {
                var p = _rep.GetUserByUsername(context.UserName);
                var claims = new[]
                    {
                        //new Claim( JwtRegisteredClaimNames.Sub, username),
                        new Claim( JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim( JwtRegisteredClaimNames.GivenName, p.UserName),
                        //new Claim( JwtRegisteredClaimNames.N, p.UserName),
                        new Claim(JwtRegisteredClaimNames.Email, context.UserName)
                    }; 
                //context.Result = new GrantValidationResult(_rep.GetUserByUsername(context.UserName).Id, "password", DateTime.Now, null, "local", null);
                context.Result = new GrantValidationResult(_rep.GetUserByUsername(context.UserName).Id, "password", DateTime.Now, claims, "local", null); 
                return Task.FromResult(context.Result);
            }else if (_rep.GetUserByUsername(context.UserName) == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid username or password", null);
                return Task.FromResult(context.Result);
            }
            else if (!_rep.EmailConfirmed(context.UserName))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Email yet to be validated", null);
                return Task.FromResult(context.Result);
            }
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid username or password", null);
            return Task.FromResult(context.Result);
        }

        //public static Claim[] GetUserClaims(User user)
        //{
        //    return new Claim[]
        //    {
        //    new Claim("user_id", user.UserId.ToString() ?? ""),
        //    new Claim(JwtClaimTypes.Name, (!string.IsNullOrEmpty(user.Firstname) && !string.IsNullOrEmpty(user.Lastname)) ? (user.Firstname + " " + user.Lastname) : ""),
        //    new Claim(JwtClaimTypes.GivenName, user.Firstname  ?? ""),
        //    new Claim(JwtClaimTypes.FamilyName, user.Lastname  ?? ""),
        //    new Claim(JwtClaimTypes.Email, user.Email  ?? ""),
        //    new Claim("some_claim_you_want_to_see", user.Some_Data_From_User ?? ""),

        //    //roles
        //    new Claim(JwtClaimTypes.Role, user.Role)
        //    };
        //}
    }
}
