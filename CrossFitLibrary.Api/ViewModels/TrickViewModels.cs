using System;
using System.Linq;
using System.Linq.Expressions;
using CrossFitLibrary.Models;

namespace CrossFitLibrary.Api.ViewModels
{
    public static class TrickViewModels
    {
        public static readonly Func<Trick, object> Create = Projection.Compile();

        public static Expression<Func<Trick, object>> Projection =>
            trick => new
            {
                trick.Id,
                trick.Slug,
                trick.Name,
                trick.Description,
                trick.Difficulty,
                trick.Version,

                Categories = trick.TrickCategories
                    .AsQueryable() // not sure we need this
                    .Select(x => x.CategoryId)
                    .ToList(),

                Prerequisites = trick.Prerequisites
                    .AsQueryable() // not sure we need this
                    .Where(x => x.Active == true)
                    .Select(x => x.PrerequisiteId)
                    .ToList(),

                Progressions = trick.Progressions
                    .AsQueryable() //not sure we need this
                    .Where(x => x.Active == true)
                    .Select(x => x.ProgressionId)
                    .ToList()
            };
        
        
        public static readonly Func<Trick, object> CreateWithUser = ProjectionWithUser.Compile();
        public static Expression<Func<Trick, object>> ProjectionWithUser =>
            trick => new
            {
                trick.Id,
                trick.Slug,
                trick.Name,
                trick.Description,
                trick.Difficulty,
                trick.Version,
                User = UserViewModels.CreateFlat(trick.User),

                Categories = trick.TrickCategories
                    .AsQueryable() // not sure we need this
                    .Select(x => x.CategoryId)
                    .ToList(),

                Prerequisites = trick.Prerequisites
                    .AsQueryable() // not sure we need this
                    .Where(x => x.Active == true)
                    .Select(x => x.PrerequisiteId)
                    .ToList(),

                Progressions = trick.Progressions
                    .AsQueryable() //not sure we need this
                    .Where(x => x.Active == true)
                    .Select(x => x.ProgressionId)
                    .ToList()
            };

        public static readonly Func<Trick, object> CreateFull = FullProjection.Compile();
        public static Expression<Func<Trick, object>> FullProjection =>
            trick => new
            {
                trick.Id,
                trick.Slug,
                trick.Name,
                trick.Description,
                trick.Difficulty,
                trick.Version,
                User = UserViewModels.CreateFlat(trick.User),
                
                Categories = trick.TrickCategories
                    .AsQueryable() // not sure we need this
                    .Select(x => x.CategoryId)
                    .ToList(),

                Prerequisites = trick.Prerequisites
                    .AsQueryable() // not sure we need this
                    .Select(x => x.PrerequisiteId)
                    .ToList(),

                Progressions = trick.Progressions
                    .AsQueryable() //not sure we need this
                    .Select(x => x.ProgressionId)
                    .ToList()
            };
    }
}