using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CrossFitLibrary.Api.ViewModels;
using CrossFitLibrary.Data;
using CrossFitLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrossFitLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public CommentController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("{id}/replies")]
        public IEnumerable<object> GetReplies(int id)
        {
            return _ctx.Comments
                .Where(x => x.ParentId == id)
                .Select(CommentViewModel.Projection)
                .ToList();
        }

        [HttpPost("{id}/replies")]
        public async Task<IActionResult> Reply(int id, [FromBody] Comment reply)
        {
            // this is  the comment we are adding a reply to
            var comment = _ctx.Comments.FirstOrDefault(x => x.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            var regex = new Regex(@"\B(?<tag>@[\w\d-]+)");

            reply.HtmlContent = reply.Content;

            foreach (Match match in regex.Matches(reply.Content))
            {
                var tag = match.Groups["tag"].Value;
                reply.HtmlContent = reply.HtmlContent.Replace(
                    tag,
                    $"<a href=\"/users/{tag.Substring(1)}\">{tag}</a>");
            }

            comment.Replies.Add(reply);
            await _ctx.SaveChangesAsync();
            return Ok(CommentViewModel.Create(reply));
        }
    }
}