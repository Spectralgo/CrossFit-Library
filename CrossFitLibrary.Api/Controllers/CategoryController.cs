using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossFitLibrary.Data;
using CrossFitLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrossFitLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public CategoryController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IEnumerable<Category> All()
        {
            return _ctx.Categories.ToList();
        }

        [HttpGet("{id}")]
        public Category Get(string id)
        {
            return _ctx.Categories.FirstOrDefault(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));
        }

        [HttpGet("{Id}/tricks")]
        public IEnumerable<Trick> ListCategoryTricks(string Id)
        {
            var result = _ctx.TrickCategories
                .Include(x => x.Trick)
                
                .Where(x => x.Id.Equals(Id, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Trick).ToList();
            return result;
        }

        [HttpPost]
        public async Task<Category> Create([FromBody] Category category)
        {
            category.Id = category.Name.Replace(" ", "-").ToLowerInvariant();
            _ctx.Add(category);
            await _ctx.SaveChangesAsync();
            return category;
        }

    }
}