using System.Linq;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;

namespace CrossFitLibrary.Api.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    protected string UserId => GetClaim(JwtClaimTypes.Subject);
    protected string UserName => GetClaim(JwtClaimTypes.PreferredUserName);
    
    private string GetClaim(string claimType) =>  User.Claims.FirstOrDefault(x => x.Type.Equals(claimType))?.Value;
}