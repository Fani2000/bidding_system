using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[] { new IdentityResources.OpenId(), new IdentityResources.Profile(), };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[] { new ApiScope("auctionApp", "Acution app full access") };

    public static IEnumerable<Client> Clients(IConfiguration config) =>
        new Client[]
        {
            new Client
            {
                ClientId = "postman",
                ClientName = "Postman",
                AllowedScopes = { "auctionApp", "openid", "profile" },
                RedirectUris = { "https://oauth.pstmn.io/v1/callback" },
                ClientSecrets = new[] { new Secret("NotASecret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword
            },
            new Client
            {
                ClientId = "nextApp",
                ClientName = "NextApp",
                AllowedScopes = { "auctionApp", "openid", "profile" },
                RedirectUris = { config["ClientApp"] + "/api/auth/callback/id-server" },
                AccessTokenLifetime = 3600 * 24 * 30, // 30 days
                ClientSecrets = new[] { new Secret("secrets".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                AlwaysIncludeUserClaimsInIdToken = true
            },
        };
}
