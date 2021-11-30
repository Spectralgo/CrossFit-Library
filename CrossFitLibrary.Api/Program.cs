using System.Collections.Generic;
using CrossFitLibrary.Data;
using CrossFitLibrary.Models;
using Microsoft.AspNetCore.Hosting;
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
}