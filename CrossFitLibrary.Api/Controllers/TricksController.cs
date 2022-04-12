using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossFitLibrary.Api.Form;
using CrossFitLibrary.Api.ViewModels;
using CrossFitLibrary.Data;
using CrossFitLibrary.Models;
using CrossFitLibrary.Models.Moderation;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CrossFitLibrary.Api.Controllers
{
    [Route("api/tricks")]
    public class TricksController : ApiController
    {
        private readonly AppDbContext _ctx;

        public TricksController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IEnumerable<object> All()
        {
            return _ctx.Tricks
                .AsNoTracking()
                .Where(x => x.Active == true)
                .Include(x => x.TrickCategories)
                .Include(x => x.Prerequisites)
                .Include(x => x.Progressions)
                .Include(x => x.User)
                .Select(TrickViewModels.ProjectionWithUser).ToList();
        }


        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var query = _ctx.Tricks.AsQueryable();

            if (int.TryParse(id, out var intId))
            {
                query = query.Where(x => x.Id == intId);
            }
            else
            {
                query = query.Where(x => x.Slug.Equals(id, StringComparison.InvariantCultureIgnoreCase) && x.Active);
            }

            var trick = query
                .Include(x => x.TrickCategories)
                .Include(x => x.Prerequisites)
                .Include(x => x.Progressions)
                .Include(x => x.User)
                .Select(TrickViewModels.FullProjection)
                .FirstOrDefault();

            if (trick is null)
            {
                return NoContent();
            }

            return Ok(trick);
        }

        [HttpGet("{trickId}/submissions")]
        public IEnumerable<object> GetSub(string trickId)
        {
            var result = _ctx.Submissions
                .Include(x => x.Video)
                .Include(x => x.User)
                .Where(x => x.TrickId.Equals(trickId, StringComparison.InvariantCultureIgnoreCase))
                .Select(SubmissionViewModels.Projection)
                .ToList();
            
            return result;
        }

        [HttpPost]
        [Authorize(Policy = CrossFitLibraryConstants.Policies.User)]
        public async Task<object> Create([FromBody] TrickForm trickForm)
        {
            var trick = new Trick
            {
                Slug = trickForm.Name.Replace(" ", "-").ToLowerInvariant(),
                Version = 1,
                Active = false,
                Name = trickForm.Name,
                Description = trickForm.Description,
                Difficulty = trickForm.Difficulty,
                TrickCategories = trickForm.Categories.Select(x => new TrickCategory { CategoryId = x }).ToList(),
                Prerequisites = trickForm.Prerequisites.Select(x => new TrickRelationship { PrerequisiteId = x })
                    .ToList(),
                Progressions = trickForm.Progressions.Select(x => new TrickRelationship { ProgressionId = x }).ToList(),
                UserId = UserId,
                
            };

            _ctx.Add(trick);
            await _ctx.SaveChangesAsync();

            _ctx.Add(new ModerationItem
            {
                Target = trick.Id,
                Type = ModerationItemTypes.Trick
            });

            await _ctx.SaveChangesAsync();
            return TrickViewModels.Create(trick);
        }

        [HttpPut]
        [Authorize(Policy = CrossFitLibraryConstants.Policies.User)]
        public async Task<IActionResult> Update([FromBody] TrickForm trickForm)
        {
            var trick = _ctx.Tricks
                .FirstOrDefault(x => x.Id == trickForm.Id);

            if (trick == null)
            {
                return NoContent();
            }

            var newTrick = new Trick
            {
                Slug = trick.Slug,
                Name = trick.Name,
                Version = trick.Version + 1,
                Description = trickForm.Description,
                Difficulty = trickForm.Difficulty,
                TrickCategories = trickForm.Categories.Select(x => new TrickCategory { CategoryId = x }).ToList(),
                Prerequisites = trickForm.Prerequisites.Select(x => new TrickRelationship { PrerequisiteId = x })
                    .ToList(),
                Progressions = trickForm.Progressions.Select(x => new TrickRelationship { ProgressionId = x }).ToList(),
                UserId = UserId,
            };


            _ctx.Add(newTrick);
            await _ctx.SaveChangesAsync();

            _ctx.Add(new ModerationItem
            {
                Current = trick.Id,
                Target = newTrick.Id,
                Type = ModerationItemTypes.Trick
            });

            await _ctx.SaveChangesAsync();

            return Ok(TrickViewModels.Create(newTrick));
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var trick = _ctx.Tricks.FirstOrDefault(x => x.Slug.Equals(id));
            trick.Deleted = true;
            return Ok();
        }
    }
}