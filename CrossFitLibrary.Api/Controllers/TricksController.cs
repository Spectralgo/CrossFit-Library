using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossFitLibrary.Data;
using CrossFitLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
        public IEnumerable<Trick> All()
        {
            return _ctx.Tricks.ToList();
        }

        [HttpGet("{id}")]
        public Trick Get(string id)
        {
            return _ctx.Tricks.FirstOrDefault(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));
        }

        [HttpGet("{trickId}/submissions")]
        public IEnumerable<Submission> GetSub(string trickId)
        {
            var result =  _ctx.Submissions.Where(x => x.TrickId.Equals(trickId, StringComparison.InvariantCultureIgnoreCase)).ToList();
            return result;
        }

        [HttpPost]
        public async Task<Trick> Create([FromBody] Trick trick)
        {
            trick.Id = trick.TrickName.Replace(" ","-").ToLowerInvariant();
            _ctx.Add(trick);
            await _ctx.SaveChangesAsync();
            return trick;
        }

        [HttpPut]
        public async Task<Trick> Put([FromBody] Trick trick)
        {
            if (string.IsNullOrEmpty(trick.Id))
            {
                return null;
            }

            _ctx.Add(trick);
            await _ctx.SaveChangesAsync();
            return trick;
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