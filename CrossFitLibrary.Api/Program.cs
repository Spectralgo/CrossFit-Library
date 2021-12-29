using System;
using System.Collections.Generic;
using System.Security.Claims;
using CrossFitLibrary.Data;
using CrossFitLibrary.Models;
using CrossFitLibrary.Models.Moderation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CrossFitLibrary.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Initialize the database by seeding it with some data
            using (var scope = host.Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

                if (env.IsDevelopment())
                {
                    ctx.Add(new Difficulty { Id = "easy", Name = "Easy", Description = "Super easy to do test" });
                    ctx.Add(new Difficulty { Id = "hard", Name = "Hard", Description = "Hard Test" });

                    ctx.Add(new Category
                        { Id = "gym", Name = "Gym", Description = "This Gym Test" });
                    ctx.Add(new Category
                        { Id = "weight-lifting", Name = "Weight Lifting", Description = "This is heavy shit test" });
                    ctx.Add(new Category
                        { Id = "conditioning", Name = "Conditioning", Description = "You will suffer test" });

                    ctx.Add(new Trick
                    {
                        Id = "snatch", Name = "Snatch",
                        Description = "Snatch is from the floor to the overhead test", Difficulty = "easy",
                        TrickCategories = new List<TrickCategory>
                            { new() { CategoryId = "gym" }, new() { CategoryId = "weight-lifting" } }
                    });
                    ctx.Add(new Trick
                    {
                        Id = "clean", Name = "Clean", Description = "Pull the bar from the floor to your shoulders",
                        Difficulty = "easy",
                        TrickCategories = new List<TrickCategory>
                            { new() { CategoryId = "weight-lifting" } }
                    });
                    ctx.Add(new Trick
                    {
                        Id = "clean-and-jerk", Name = "Clean and jerk",
                        Description = "A clean with a finish overhead", Difficulty = "hard",
                        TrickCategories = new List<TrickCategory>
                            { new() { CategoryId = "conditioning" }, new() { CategoryId = "weight-lifting" } },
                        Prerequisites = new List<TrickRelationship>
                        {
                            new() { PrerequisiteId = "clean" }
                        }
                    });

                    ctx.Add(new Submission
                    {
                        TrickId = "snatch",
                        Description = "I'm just trying my best",
                        Video = new Video
                        {
                            VideoLink = "one.mp4",
                            ThumbnailLink = "one.jpg"
                        },
                        VideoProcessed = true
                    });
                    ctx.Add(new Submission
                    {
                        TrickId = "clean",
                        Description = "best clean of all time",
                        Video = new Video
                        {
                            VideoLink = "two.mp4",
                            ThumbnailLink = "two.jpg"
                        },
                        VideoProcessed = true
                    });
                    ctx.Add(new ModerationItem
                        {
                            Target = "snatch",
                            Type = ModerationItemTypes.Trick,
                        }
                    );
                    ctx.Add(new Comment
                    {
                        Content = "This is a comment, I hope it works, I'm just trying to see if it works",
                        HtmlContent = "This is a comment, I hope it works, I'm just trying to see if it works",
                        // TrickId = "snatch",
                        ModerationItemId = 1,
                        DateOfCreation = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")

                    });
                    ctx.Add(new Comment
                    {
                        Content = "A comment related to a trick",
                        HtmlContent = "A comment related to a trick",
                        TrickId = "snatch",
                        DateOfCreation = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
                    });
                    
                    ctx.SaveChanges();


                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                    var user = new IdentityUser("test@test.com");
                    userManager.CreateAsync(user, "password").GetAwaiter().GetResult();

                    var mod = new IdentityUser("mod@test.com");
                    userManager.CreateAsync(mod, "password").GetAwaiter().GetResult();
                    userManager
                        .AddClaimAsync(mod, new Claim(Startup.TrickingLibraryConstants.Claims.Role, Startup.TrickingLibraryConstants.Roles.Mod))
                        .GetAwaiter()
                        .GetResult();

                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}