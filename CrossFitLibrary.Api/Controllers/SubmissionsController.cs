using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using CrossFitLibrary.Api.BackgroundServices;
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
            return _ctx.Submissions.Where(x => x.VideoProcessed == true).ToList();
        }

        [HttpGet("{id}")]
        public Submission Get(int id)
        {
            return _ctx.Submissions.FirstOrDefault(x => x.Id.Equals(id));
        }


        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] Submission submission,
            [FromServices] Channel<EditVideoChannelMessage> channel,
            [FromServices] VideoManager videoManager)
        {

            if (!videoManager.TemporaryVideoExists(submission.VideoFileName))
            {
                return BadRequest();
            }
            submission.VideoProcessed = false;
            _ctx.Add(submission);
            await _ctx.SaveChangesAsync();

            //sends a message to background service to edit/convert video using the channel
            var submissionInfoToProcessTheVideo = new EditVideoChannelMessage
            {
                SubmissionId = submission.Id,
                VideoFileName = submission.VideoFileName,
            };

            channel.Writer.WriteAsync(submissionInfoToProcessTheVideo);
            
            return Ok(submission);
        }

        [HttpPut]
        public async Task<Submission> Update([FromBody] Submission submission)
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