using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossFitLibrary.Data;
using CrossFitLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrossFitLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/difficulties")]
    public class DifficultyController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public DifficultyController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public List<Difficulty> All()
        {
            return _ctx.Difficulties.ToList();
        }

        [HttpGet("{id}")]
        public Difficulty Get(string id)
        {
            return _ctx.Difficulties.FirstOrDefault(x => x.Slug.Equals(id, StringComparison.InvariantCultureIgnoreCase));
        }

        [HttpGet("{id}/tricks")]
        public IEnumerable<Trick> ListDifficultyTricks(string id)
        {
            var result = _ctx.Tricks.Where(x => x.Difficulty.Equals(id, StringComparison.InvariantCultureIgnoreCase)).ToList();
            return result;
        }

        [HttpPost]
        public async Task<Difficulty> Create([FromBody] Difficulty difficulty)
        {
            difficulty.Slug = difficulty.Name.Replace(" ", "-").ToLowerInvariant();
            _ctx.Add(difficulty);
            await _ctx.SaveChangesAsync();
            return difficulty;
        }
    }
}