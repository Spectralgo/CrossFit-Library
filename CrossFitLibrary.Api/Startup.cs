using System.Threading.Channels;
using CrossFitLibrary.Api.BackgroundServices;
using CrossFitLibrary.Data;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CrossFitLibrary.Api;

public class Startup
{
    private const string AllCors = "All";
    private readonly IWebHostEnvironment _env;

    public Startup(IWebHostEnvironment env)
    {
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
            .AddSingleton<VideoManager>()
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

        services.ConfigureApplicationCookie(config => { config.LoginPath = "/Account/Login"; });


        var identityServerBuilder = services.AddIdentityServer();
        identityServerBuilder.AddAspNetIdentity<IdentityUser>();

        if (_env.IsDevelopment())
        {
            identityServerBuilder.AddInMemoryIdentityResources(new IdentityResource[]
            { //scopes
                new IdentityResources.OpenId(), //user identification
                new IdentityResources.Profile() //user personal info
            });

            identityServerBuilder.AddInMemoryClients(new[]
            {
                new Client
                {
                    ClientId = "crossfit-library-client",
                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = new[] { "http://localhost:3000" },
                    PostLogoutRedirectUris = new[] { "http://localhost:3000" },
                    AllowedCorsOrigins = new[] { "http://localhost:3000" },
                    
                    AllowedScopes = new[]
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },

                    RequirePkce = true,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RequireClientSecret = false
                }
            });
            identityServerBuilder.AddDeveloperSigningCredential();
        }
        
    }
}