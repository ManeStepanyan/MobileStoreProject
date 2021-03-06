﻿using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace AuthenticationServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("UserAPI"),
                  new ApiResource("OrderAPI"),
                      new ApiResource("CatalogAPI"),
                        new ApiResource("WebClient")

            };
        }


        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "SuperAdmin",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    ClientClaimsPrefix = "",
                    AllowedScopes = { "UserAPI" ,"OrderAPI","CatalogAPI","WebClient","openid"},
                    AccessTokenLifetime = 3600,
                },

            };

        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = JwtClaimTypes.Role,
                    UserClaims = new List<string> { JwtClaimTypes.Role }
                }
            };

        }
    }
}

