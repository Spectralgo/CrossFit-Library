using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossFitLibrary.Api.BackgroundServices;
using CrossFitLibrary.Api.BackgroundServices.VideoEditing;
using CrossFitLibrary.Api.Settings;
using CrossFitLibrary.Data;
using CrossFitLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace CrossFitLibrary.Api.Controllers;

[Route("api/users")]
[Authorize(CrossFitLibraryConstants.Policies.User)]
public class UserController : ApiController
{
    private readonly AppDbContext _ctx;

    public UserController(AppDbContext ctx)
    {
        _ctx = ctx;
    }


    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest();
        }

        var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId));

        if (user is not null)
        {
            return Ok(user);
        }

        user = new User
        {
            Id = UserId,
            Username = UserName
        };

        _ctx.Add(user);
        await _ctx.SaveChangesAsync();

        return Ok(user);
    }


    [HttpGet("{id}")]
    public IActionResult GetUser(string id)
    {
        return Ok();
    }


    [HttpGet("{id}/submissions")]
    public Task<List<Submission>> GetUserSubmissions(string id)
    {
        return _ctx.Submissions.Include(x => x.Video)
            .Where(x => x.UserId.Equals(id))
            .ToListAsync();
    }

    [HttpPut("me/image")]
    public async Task<IActionResult> ChangeUserAvatarImage(
        IFormFile imageFile,
        [FromServices] IFileManager fileManager
        )
    {
        if (imageFile == null) return BadRequest();

        var userId = UserId;
        if (string.IsNullOrEmpty(userId)) return BadRequest();

        var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId));
        if (user == null) return NoContent();

        var imageFileName = CrossFitLibraryConstants.Files.GenerateImageFileName();
        await using (var stream = System.IO.File.Create(fileManager.GetSavePath(imageFileName)))
        using (var imageProcessor = await Image.LoadAsync(imageFile.OpenReadStream()))
        {
            imageProcessor.Mutate(x => x.Resize(48, 48));

            await imageProcessor.SaveAsync(stream, new JpegEncoder());
        }

        user.ImageUrl = fileManager.GetFileUrl(imageFileName, FileType.Image) ;
        await _ctx.SaveChangesAsync();
        return Ok(user);
    }
}