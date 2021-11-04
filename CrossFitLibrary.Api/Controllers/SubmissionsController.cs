using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossFitLibrary.Data;
using CrossFitLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrossFitLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/submissions")]
    public class SubmissionsController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public SubmissionsController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IEnumerable<Submission> All()
        {
            return _ctx.Submissions.ToList();
        }

        [HttpGet("{id}")]
        public Submission Get(int id)
        {
            return _ctx.Submissions.FirstOrDefault(x => x.Id.Equals(id));
        }


        [HttpPost]
        public async Task<Submission> Create([FromBody] Submission submission)
        {
            _ctx.Add(submission);
            await _ctx.SaveChangesAsync();
            return submission;
            
        }

        [HttpPut]
        public async Task<Submission> Put([FromBody] Submission submission)
        {
            if (submission.Id == 0)
            {
                return null;
            }

            _ctx.Add(submission);
            await _ctx.SaveChangesAsync();
            return submission;
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var submission = _ctx.Submissions.FirstOrDefault(x => x.Id.Equals(id));
            submission.Deleted = true;
            return Ok();
        }
    }
}
