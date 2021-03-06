using System.Security.Claims;
using System.Threading.Channels;
using CrossFitLibrary.Api.BackgroundServices;
using CrossFitLibrary.Api.BackgroundServices.VideoEditing;
using CrossFitLibrary.Data;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CrossFitLibrary.Api;

public class Startup
{
    private const string AllCors = "All";
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;

    public Startup(IConfiguration config, IWebHostEnvironment env)
    {
        _config = config;
        _env = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddControllers();

        services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Dev"));
        AddIdentity(services);


        services.AddHostedService<VideoEditingBackgroundService>()
            .AddSingleton(_ => Channel.CreateUnbounded<EditVideoChannelMessage>())
            .AddScoped<VersionMigrationContext>()
            .AddFileManager(_config)
            .AddCors(options =>
                options.AddPolicy(AllCors, build => build
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowAnyMethod()));
    }

    public void Configure(IApplicationBuilder app)
    {
        if (_env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseCors(AllCors);

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseIdentityServer();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapRazorPages();
        });
    }

    private void AddIdentity(IServiceCollection services)
    {
        services.AddDbContext<IdentityDbContext>(config =>
            config.UseInMemoryDatabase("DevIdentity")); // we are using a different Db context for identities 

        services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

                if (_env.IsDevelopment())
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                }
                else
                {
                    // Todo: configure for production
                }
            })
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(config =>
        {
            config.LoginPath = "/Account/Login";
            config.LogoutPath = "/api/auth/logout";
        });


        var identityServerBuilder = services.AddIdentityServer();
        identityServerBuilder.AddAspNetIdentity<IdentityUser>();

        if (_env.IsDevelopment())
        {
            identityServerBuilder.AddInMemoryIdentityResources(new IdentityResource[]
            {
                //scopes
                new IdentityResources.OpenId(), //user identification
                new IdentityResources.Profile(), //user personal info
                new IdentityResource(CrossFitLibraryConstants.IdentityResources.RoleScope,
                    new[] { CrossFitLibraryConstants.Claims.Role })
            });

            identityServerBuilder.AddInMemoryApiScopes(new ApiScope[]
            {
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName, new[]
                {
                    JwtClaimTypes.PreferredUserName,
                    CrossFitLibraryConstants.Claims.Role
                })
            });


            identityServerBuilder.AddInMemoryClients(new[]
            {
                new Client
                {
                    ClientId = "crossfit-library-client",
                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = new[]
                    {
                        "https://localhost:3000/oidc/sign-in-callback.html",
                        "https://localhost:3000/oidc/sign-in-silent-callback.html"
                    },

                    PostLogoutRedirectUris = new[] { "https://localhost:3000" },
                    AllowedCorsOrigins = new[] { "https://localhost:3000" },

                    AllowedScopes = new[]
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.LocalApi.ScopeName,
                        CrossFitLibraryConstants.IdentityResources.RoleScope
                    },

                    RequirePkce = true,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RequireClientSecret = false
                }
            });


            identityServerBuilder.AddDeveloperSigningCredential();
        }

        services.AddLocalApiAuthentication();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(CrossFitLibraryConstants.Policies.Mod, policy =>
            {
                var is4Policy = options.GetPolicy(IdentityServerConstants.LocalApi.PolicyName);
                policy.Combine(is4Policy);
                policy.RequireClaim(CrossFitLibraryConstants.Claims.Role,
                    CrossFitLibraryConstants.Roles.Mod);
            });
        });
    }
}