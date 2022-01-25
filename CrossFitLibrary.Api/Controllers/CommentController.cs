using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CrossFitLibrary.Api.ViewModels;
using CrossFitLibrary.Data;
using CrossFitLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            reply.DateOfCreation = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            comment.Replies.Add(reply);
            await _ctx.SaveChangesAsync();
            return Ok(CommentViewModel.Create(reply));
        }

       // Tricks Section  //
        [HttpGet("{id}/tricks")]
        public IEnumerable<object> GetTrickComments(string id)
        {
            return  _ctx.Comments
                .Where(x => x.TrickId.Equals(id))
                .Select(CommentViewModel.Projection);
        }

        [HttpPost("{id}/tricks")]
        public async Task<IActionResult> CreateCommentForTrick(string id, [FromBody] Comment newComment)
        {
            // this is  the trick we are adding a reply to
            var trick = _ctx.Tricks.FirstOrDefault(x => x.Slug.Equals(id) );

            if (trick == null)
            {
                return NotFound();
            }

            var regex = new Regex(@"\B(?<tag>@[\w\d-]+)");

            newComment.HtmlContent = newComment.Content;

            foreach (Match match in regex.Matches(newComment.Content))
            {
                var tag = match.Groups["tag"].Value;
                newComment.HtmlContent = newComment.HtmlContent.Replace(
                    tag,
                    $"<a href=\"/users/{tag.Substring(1)}\">{tag}</a>");
            }

            
            newComment.DateOfCreation = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            trick.Comments.Add(newComment);
            await _ctx.SaveChangesAsync();
           return Ok(CommentViewModel.Create(newComment));
        }

       // Submissions Section  //
        [HttpGet("{id}/submissions")]
        public IEnumerable<object> GetSubmissionComments(int id)
        {
            return  _ctx.Comments
                .Where(x => x.SubmissionId == id)
                .Select(CommentViewModel.Projection);
        }

        [HttpPost("{id}/submissions")]
        public async Task<IActionResult> CreateCommentForSubmission(int id, [FromBody] Comment newComment)
        {
            // this is  the submission we are adding a reply to
            var submission = _ctx.Submissions.FirstOrDefault(x => x.Id == id );

            if (submission == null)
            {
                return NotFound();
            }

            var regex = new Regex(@"\B(?<tag>@[\w\d-]+)");

            newComment.HtmlContent = newComment.Content;

            foreach (Match match in regex.Matches(newComment.Content))
            {
                var tag = match.Groups["tag"].Value;
                newComment.HtmlContent = newComment.HtmlContent.Replace(
                    tag,
                    $"<a href=\"/users/{tag.Substring(1)}\">{tag}</a>");
            }

            newComment.DateOfCreation = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");;
            submission.Comments.Add(newComment);
            await _ctx.SaveChangesAsync();
           return Ok(CommentViewModel.Create(newComment));
        }


    }
}