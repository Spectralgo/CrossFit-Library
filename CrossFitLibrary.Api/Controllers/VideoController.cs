using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CrossFitLibrary.Api.BackgroundServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CrossFitLibrary.Api.Controllers
{
    [Route("api/video")]
    public class VideoController : ControllerBase
    {
        private readonly VideoManager _videoManager;

        public VideoController(VideoManager videoManager)
        {
            _videoManager = videoManager;
        }


        [HttpGet("{videoFileName}")]
        public IActionResult GetVideofile(string videoFileName)
        {
            
            var video_path_during_dev = _videoManager.GetSavePath(videoFileName);
            if (string.IsNullOrEmpty(video_path_during_dev))
            {
                return BadRequest();
            }
            var fileStream = new FileStream(video_path_during_dev, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, "video/*");
        }

        [HttpPost]
        public Task<string> UploadVideo(IFormFile video)
        { 
            // Returns temporary video file name to the client before conversion.
            // It helps identify the temp file to delete if the conversion fails or if the user cancels the upload"
            return _videoManager.SaveTemporaryVideo(video);
        }
        
        [HttpDelete("{videoFileName}")]
        public IActionResult DeleteTemporaryVideo(string videoFileName)
        {
            
            if (!_videoManager.IsTemporaryFile(videoFileName))
            {
                return BadRequest();
            }

            
            
            if (!_videoManager.FileExists(videoFileName))
            {
                return NoContent();
            }
            
            _videoManager.DeleteFile(videoFileName);
            
            return Ok();
        }
    }
}