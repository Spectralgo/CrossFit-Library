using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossFitLibrary.Api.Form;
using CrossFitLibrary.Api.ViewModels;
using CrossFitLibrary.Data;
using CrossFitLibrary.Models;
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
            return _ctx.Tricks.Select(TrickViewModels.Projection).ToList();
        }
        
        [HttpGet("test")]
        [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName )] 
        public string TestAuth()
        {
            return "test";
        }
        
        
        [HttpGet("mod")]
        [Authorize(Policy = Startup.TrickingLibraryConstants.Policies.Mod)] 
        public string ModAuth()
        {
            return "Mod test";
        }
        

        [HttpGet("{id}")]
        public object Get(string id)
        {
            return _ctx.Tricks
                .Where(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase))
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
                Id = trickForm.Name.Replace(" ", "-").ToLowerInvariant(),
                Name = trickForm.Name,
                Description = trickForm.Description,
                Difficulty = trickForm.Difficulty,
                TrickCategories = trickForm.Categories.Select(x => new TrickCategory { CategoryId = x }).ToList(),
                Prerequisites = trickForm.Prerequisites.Select(x => new TrickRelationship { PrerequisiteId = x }).ToList(),
                Progressions = trickForm.Progressions.Select(x => new TrickRelationship { ProgressionId = x }).ToList(),
            };

            _ctx.Add(trick);
            await _ctx.SaveChangesAsync();
            return TrickViewModels.Create(trick);
        }

        [HttpPut]
        public async Task<object> Update([FromBody] Trick trick)

        {
            if (string.IsNullOrEmpty(trick.Id))
            {
                return null;
            }

            _ctx.Add(trick);
            await _ctx.SaveChangesAsync();
            return TrickViewModels.Create(trick);
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var trick = _ctx.Tricks.FirstOrDefault(x => x.Id.Equals(id));
            trick.Deleted = true;
            return Ok();
        }
    }
}