using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CrossFitLibrary.Api.ViewModels;
using CrossFitLibrary.Data;
using CrossFitLibrary.Models;
using CrossFitLibrary.Models.Moderation;
using Microsoft.AspNetCore.Mvc;

namespace CrossFitLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/moderation-items")]
    public class ModerationItemController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public ModerationItemController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IEnumerable<ModerationItem> All()
        {
            return _ctx.ModerationItems.ToList();
        }

        [HttpGet("{id}")]
        public ModerationItem Get(int id)
        {
            return _ctx.ModerationItems.FirstOrDefault(x => x.Id.Equals(id));
        }


        [HttpGet("{id}/comments")]
        public List<object> GetComments(int id)
        {
            return _ctx.Comments
                .Where(x => x.ModerationItemId.Equals(id))
                .Select(CommentViewModel.Projection)
                .ToList();
        }

        [HttpPost("{id}/comments")]
        public async Task<IActionResult> AddComment(int id, [FromBody] Comment comment)
        {

            if (!_ctx.ModerationItems.Any(x => x.Id == id))
            {
                return NoContent();
            }

            var regex = new Regex(@"\B(?<tag>@[\w\d-]+)");

            comment.HtmlContent = comment.Content;
            foreach (Match match in regex.Matches(comment.Content))
            {
                var tag = match.Groups["tag"].Value;
                comment.HtmlContent = comment.HtmlContent.Replace(
                    tag,
                    $"<a href=\"/users/{tag.Substring(1)}\">{tag}</a>");
            }


            _ctx.Add(comment);
            await _ctx.SaveChangesAsync();
            return Ok(CommentViewModel.Create(comment));
        }


        [HttpGet("{id}/reviews")]
        public List<object> GetReview(int id)
        {
            return _ctx.Reviews
                .Where(x => x.ModerationItemId.Equals(id))
                .Select(ReviewViewModel.Projection)
                .ToList();
        }

        [HttpPost("{id}/reviews")]
        public async Task<IActionResult> AddReview(int id, [FromBody] Review review)
        {

            // Be sure the moderationItem exists
            if (!_ctx.ModerationItems.Any(x => x.Id == id))
            {
                return NoContent();
            }
            
            review.ModerationItemId = id; // ? It might be already set in the frontend (FRenard 2021-12-13)
            
            _ctx.Add(review); // Is it the same as doing this _ctx.Reviews.Add(review)
            await _ctx.SaveChangesAsync();
            return Ok(ReviewViewModel.Create(review));
        }
    }
}