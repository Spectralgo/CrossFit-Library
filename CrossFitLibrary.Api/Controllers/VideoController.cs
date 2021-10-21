using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace CrossFitLibrary.Api.Controllers
{
    [Route("api/video")]
    public class VideoController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public VideoController(IWebHostEnvironment env)
        
        {
            _env = env;
        }
        
        
        [HttpGet("{video}")]
        public IActionResult getVideo(string video)
        {
            var savePath = Path.Combine(_env.WebRootPath, video);
            var fileStream = new FileStream(savePath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream,"video/*");
        }

        [HttpPost]
        public async Task<IActionResult> uploadVideo(IFormFile video)
        {
            var mime = video.FileName.Split(".").Last();
            var fileName = string.Concat(Path.GetRandomFileName(), ".", mime);
            var savePath = Path.Combine(_env.WebRootPath, fileName);

            await using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                await video.CopyToAsync(fileStream);
            }

            return Ok(fileName);
        }

    }
}