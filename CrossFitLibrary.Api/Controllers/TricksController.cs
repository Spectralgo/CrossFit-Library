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
    [ApiController]
    [Route("api/tricks")]
    public class TricksController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public TricksController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IEnumerable<object> All()
        {
            // todo add filter to only send active tricks and latest version
            return _ctx.Tricks.Select(TrickViewModels.Projection).ToList();
        }
        
        [HttpGet("test")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName )] 
        public string TestAuth()
        {
            return "test";
        }
        
        
        [HttpGet("mod")]
        [Authorize(Policy = CrossFitLibraryConstants.Policies.Mod)] 
        public string ModAuth()
        {
            return "Mod test";
        }
        

        [HttpGet("{id}")]
        public object Get(string id)
        {
            return _ctx.Tricks
                .Where(x => x.Slug.Equals(id, StringComparison.InvariantCultureIgnoreCase))
                .Select(TrickViewModels.Projection)
                .FirstOrDefault();
        }

        [HttpGet("{trickId}/submissions")]
        public IEnumerable<Submission> GetSub(string trickId)
        {
            var result = _ctx.Submissions
                .Include(x => x.Video)
                .Where(x => x.TrickId.Equals(trickId, StringComparison.InvariantCultureIgnoreCase)).ToList();
            return result;
        }

        [HttpPost]
        public async Task<object> Create([FromBody] TrickForm trickForm)
        {
            var trick = new Trick
            {
                Slug = trickForm.Name.Replace(" ", "-").ToLowerInvariant(),
                Version = 1,
                Active = true, 
                Name = trickForm.Name,
                Description = trickForm.Description,
                Difficulty = trickForm.Difficulty,
                TrickCategories = trickForm.Categories.Select(x => new TrickCategory { CategoryId = x }).ToList(),
                Prerequisites = trickForm.Prerequisites.Select(x => new TrickRelationship { PrerequisiteId = x }).ToList(),
                Progressions = trickForm.Progressions.Select(x => new TrickRelationship { ProgressionId = x }).ToList(),
            };

            _ctx.Add(new ModerationItem { Target = trick.Slug, VersionTarget = trick.Version });
            _ctx.Add(trick);
            await _ctx.SaveChangesAsync();
            return TrickViewModels.Create(trick);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TrickForm trickForm)
        {

            
            // We want to keep the old version
            var newTrick = new Trick
            {
                Slug = trickForm.Name.Replace(" ", "-").ToLowerInvariant(),
                Name = trickForm.Name,
                Description = trickForm.Description,
                Difficulty = trickForm.Difficulty,
                TrickCategories = trickForm.Categories.Select(x => new TrickCategory { CategoryId = x }).ToList(),
                Prerequisites = trickForm.Prerequisites.Select(x => new TrickRelationship { PrerequisiteId = x }).ToList(),
                Progressions = trickForm.Progressions.Select(x => new TrickRelationship { ProgressionId = x }).ToList(),
            };
            
            // Checking if the trick exist in the database
            var trick = _ctx.Tricks
                .FirstOrDefault(x => x.Slug.Equals(newTrick.Slug, StringComparison.InvariantCultureIgnoreCase));
            if (trick == null)
            {
                return NoContent();
            }
            
            // Looking for the latest version
            var trickVersion = _ctx.Tricks
                .Where(x => x.Slug.Equals(newTrick.Slug, StringComparison.InvariantCultureIgnoreCase))
                .LatestVersion(); // Custom static method from QueryExtension.cs in data CrossFitLibrary.Data project
            
            
            
            // Incrementing the latest version & adding the new trick to the moderation list
            newTrick.Version = trickVersion + 1;
            _ctx.Add(newTrick);
            _ctx.Add(new ModerationItem { Target = newTrick.Slug, VersionTarget = newTrick.Version });
            await _ctx.SaveChangesAsync();
            
            // todo redirect to the mod item istead of returning the trick
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