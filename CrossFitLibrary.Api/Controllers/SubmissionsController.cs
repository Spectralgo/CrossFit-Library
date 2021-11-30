using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using CrossFitLibrary.Api.BackgroundServices;
using CrossFitLibrary.Api.Form;
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
            [FromBody] SubmissionForm submissionForm,
            [FromServices] Channel<EditVideoChannelMessage> channel,
            [FromServices] VideoManager videoManager)
        {
            if (!videoManager.FileExists(submissionForm.VideoFileName))
            {
                return BadRequest();
            }

            var submission = new Submission
            {
                TrickId = submissionForm.TrickId,
                Description = submissionForm.Description,
                VideoProcessed = false
            };

            _ctx.Add(submission);
            await _ctx.SaveChangesAsync();

            // sends a message to background service to edit/convert video using the channel. FR
            var submissionInfoToProcessTheVideo = new EditVideoChannelMessage
            {
                // We need to pass the submission id to the background service,
                // in order to use LinQ on ctx.Submission and relocate the submission
                // after having processed/converted the video in the background. (FRenard 2021-11-30)
                 
                SubmissionId = submission.Id,
                VideoFileName = submissionForm.VideoFileName,
            };

            await channel.Writer.WriteAsync(submissionInfoToProcessTheVideo);

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