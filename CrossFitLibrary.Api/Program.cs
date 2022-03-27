using System;
using System.Collections.Generic;
using System.Security.Claims;
using CrossFitLibrary.Data;
using CrossFitLibrary.Models;
using CrossFitLibrary.Models.Moderation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CrossFitLibrary.Api;

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
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var testUser = new IdentityUser("test") { Email = "test@test.com" };
                userManager.CreateAsync(testUser, "password").GetAwaiter().GetResult();

                var mod = new IdentityUser("mod") { Email = "mod@test.com" };
                userManager.CreateAsync(mod, "password").GetAwaiter().GetResult();
                userManager
                    .AddClaimAsync(mod,
                        new Claim(CrossFitLibraryConstants.Claims.Role, CrossFitLibraryConstants.Roles.Mod))
                    .GetAwaiter()
                    .GetResult();

                ctx.Add(new Difficulty
                {
                    Id = 1,
                    Slug = "easy",
                    Name = "Easy",
                    Active = true,
                    Version = 1,
                    Description = "Super easy to do test"
                });
                ctx.Add(new Difficulty
                    { Id = 2, Slug = "hard", Name = "Hard", Active = true, Version = 1, Description = "Hard Test" });

                ctx.Add(new Category
                    { Id = 1, Slug = "gym", Name = "Gym", Active = true, Version = 1, Description = "This Gym Test" });
                ctx.Add(new Category
                {
                    Id = 2,
                    Slug = "weight-lifting",
                    Name = "Weight Lifting",
                    Active = true,
                    Version = 1,
                    Description = "This is heavy shit test"
                });
                ctx.Add(new Category
                {
                    Id = 3,
                    Slug = "conditioning",
                    Name = "Conditioning",
                    Active = true,
                    Version = 1,
                    Description = "You will suffer test"
                });

                ctx.Add(new Trick
                {
                    Id = 1,
                    Slug = "snatch",
                    Name = "Snatch",
                    Active = true,
                    Version = 1,
                    Description = "Snatch is from the floor to the overhead test",
                    Difficulty = "easy",
                    TrickCategories = new List<TrickCategory>
                        { new() { CategoryId = 1 }, new() { CategoryId = 2 } }
                });
                ctx.Add(new Trick
                {
                    Id = 2,
                    Slug = "clean",
                    Name = "Clean",
                    Active = true,
                    Version = 1,
                    Description = "Pull the bar from the floor to your shoulders",
                    Difficulty = "easy",
                    TrickCategories = new List<TrickCategory>
                        { new() { CategoryId = 2 } }
                });
                ctx.Add(new Trick
                {
                    Id = 3,
                    Slug = "clean-and-jerk",
                    Name = "Clean and jerk",
                    Active = true,
                    Version = 1,
                    Description = "A clean with a finish overhead",
                    Difficulty = "hard",
                    TrickCategories = new List<TrickCategory>
                        { new() { CategoryId = 3 }, new() { CategoryId = 2 } },
                    Prerequisites = new List<TrickRelationship>
                    {
                        new() { PrerequisiteId = 2 }
                    }
                });

                ctx.Add(new Submission
                {
                    Id = 1,
                    UserId = testUser.Id,
                    Active = true,
                    Version = 1,
                    TrickId = "snatch",
                    Description = "I'm just trying my best",
                    Video = new Video
                    {
                        VideoUrl = "https://localhost:5001/api/files/video/one.mp4",
                        ThumbnailUrl = "https://localhost:5001/api/files/image/one.jpg"
                    },
                    VideoProcessed = true
                });
                ctx.Add(new Submission
                {
                    Id = 2,
                    UserId = testUser.Id,
                    Active = true,
                    Version = 1,
                    TrickId = "clean",
                    Description = "best clean of all time",
                    Video = new Video
                    {
                        VideoUrl = "https://localhost:5001/api/files/video/two.mp4",
                        ThumbnailUrl = "https://localhost:5001/api/files/image/two.jpg"
                    },
                    VideoProcessed = true
                });
                ctx.Add(new ModerationItem
                    {
                        Id = 1,
                        Active = true,
                        Version = 1,
                        Target = 1,
                        Type = ModerationItemTypes.Trick
                    }
                );
                ctx.Add(new Comment
                {
                    Id = 1,
                    Active = true,
                    Version = 1,
                    Content = "This is a comment, I hope it works, I'm just trying to see if it works",
                    HtmlContent = "This is a comment, I hope it works, I'm just trying to see if it works",
                    ModerationItemId = 1,
                    DateOfCreation = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
                });
                ctx.Add(new Comment
                {
                    Id = 2,
                    Active = true,
                    Version = 1,
                    Content = "A comment related to a trick",
                    HtmlContent = "A comment related to a trick",
                    TrickId = "snatch",
                    DateOfCreation = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
                });

                ctx.SaveChanges();
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