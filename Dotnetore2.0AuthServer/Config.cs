using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models; 

namespace DotnetCore2.AuthServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            var openIdScope = new IdentityResources.OpenId();
            openIdScope.UserClaims.Add(JwtClaimTypes.Locale); 

            return new List<IdentityResource>
            {
                openIdScope,
                //new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                //new IdentityResource(Constants.RolesScopeType, Constants.RolesScopeType,
                //new List<string> {JwtClaimTypes.Role, Constants.TenantIdClaimType})
                //{
                //    //when false (default), the user can deselect the scope on consent screen 
                //    Required = true
                //}
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("vbIncAppApi", "Resource API")
                {
                    Scopes = {new Scope("api.read")}
                }
            };
        }

        //public static IEnumerable<Client> GetClients()
        //{
        //    return new[]
        //    {
        //        new Client {
        //            RequireConsent = false,
        //            ClientId = "angular_spa",
        //            ClientName = "Angular SPA",
        //            AllowedGrantTypes = GrantTypes.Implicit,//GrantTypes.ResourceOwnerPassword, //
        //            AllowedScopes = { "openid", "profile", "email", "api.read" },
        //            RedirectUris = {"http://localhost:4200/auth-callback"},
        //            PostLogoutRedirectUris = {"http://localhost:4200/"}, 
        //            //AllowedCorsOrigins = {"http://localhost:4200"},
        //            AllowAccessTokensViaBrowser = true,
        //            AccessTokenLifetime = 3600
        //        }
        //    };
        //}

        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                
                // resource owner password grant client
                new Client
                {
                    ClientId = "vbIncApp",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secretkey_secretkey123!".Sha256())
                    },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address,
                        "vbIncAppApi"
                    },
                    AllowedCorsOrigins = new List<string> {"http://localhost:4200"}
                    ,RefreshTokenUsage = TokenUsage.ReUse
                    ,SlidingRefreshTokenLifetime = 60
                    ,RefreshTokenExpiration = TokenExpiration.Sliding
                    //,
                    //AllowedCorsOrigins ={"http://localhost:5000",
                    //"https://localhost:5001","http://localhost:5002",
                    //"https://localhost:5003","http://localhost:4200",
                    //"http://vacantboards.ca",
                    //"http://vacantboards.com",
                    //"https://vacantboards.com",
                    //"https://vacantboards.ca"}
                }
            };
        }
    }
}
