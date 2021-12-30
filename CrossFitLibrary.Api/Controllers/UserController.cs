using System.Linq;
using System.Threading.Tasks;
using CrossFitLibrary.Data;
using CrossFitLibrary.Models;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrossFitLibrary.Api.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _ctx;

    public UserController(AppDbContext ctx)
    {
        _ctx = ctx;
    }


    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = User.Claims
            .FirstOrDefault(x => x.Type.Equals(JwtClaimTypes.Subject))?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest();
        }

        var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId));

        if (user is not null)
        {
            return Ok(user);
        }

        user = new User()
        {
            Id = userId,
            Username = 
         User.Claims
            .FirstOrDefault(x => x.Type.Equals(JwtClaimTypes.PreferredUserName))?.Value
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

    // [HttpPost]
    // public IActionResult CreateUser([FromBody] Object user) => Ok();
}