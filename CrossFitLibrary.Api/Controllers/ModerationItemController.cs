using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CrossFitLibrary.Api.Form;
using CrossFitLibrary.Api.ViewModels;
using CrossFitLibrary.Data;
using CrossFitLibrary.Models;
using CrossFitLibrary.Models.Moderation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return _ctx.ModerationItems
                .Where(x => x.Deleted == false)
                .ToList();
        }

        [HttpGet("{id}")]
        public object Get(int id)
        {
            return _ctx.ModerationItems
                .Include(x => x.Comments)
                .Include(x => x.Reviews)
                .Where(x => x.Id.Equals(id))
                .Select(ModerationItemViewModels.Projection)
                .FirstOrDefault();

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
        public async Task<IActionResult> AddReview(
            int id,
            [FromBody] ReviewForm reviewForm,
            [FromServices] VersionMigrationContext migrationContext )
        {

            var modItem = _ctx.ModerationItems
                .Include(x => x.Reviews)
                .FirstOrDefault(x => x.Id == id);
            
            // Be sure the moderationItem exists
            if (modItem == null)
            {
                return NoContent();
            }

            if (modItem.Deleted)
            {
                return BadRequest("Moderation item no longer exists.");
            }

            // TODO: make this async safe
            // because if two reviews comes at the same time the review count will only amount to 2
            var review = new Review
            {
                ModerationItemId = id,
                Comment = reviewForm.Comment,
                Status = reviewForm.Status
            };
            
            _ctx.Add(review); // Same as doing this _ctx.Reviews.Add(review)

            
            // TODO: use configuration replace the magic '3'
            if (modItem.Reviews.Count >= 3)
            {
                 migrationContext.Migrate(modItem);
                 modItem.Deleted = true;
            }
            
            await _ctx.SaveChangesAsync();
            return Ok(ReviewViewModel.Create(review));
        }
    }
}