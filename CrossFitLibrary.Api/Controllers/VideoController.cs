﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CrossFitLibrary.Api.BackgroundServices;
using CrossFitLibrary.Api.BackgroundServices.VideoEditing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CrossFitLibrary.Api.Controllers
{
    [Route("api/video")]
    public class VideoController : ControllerBase
    {
        private readonly IFileManager _fileManagerLocal;

        public VideoController(IFileManager fileManagerLocal)
        {
            _fileManagerLocal = fileManagerLocal;
        }


        [HttpGet("{videoFileName}")]
        public IActionResult GetVideofile(string videoFileName)
        {
            
            var video_path_during_dev = _fileManagerLocal.GetSavePath(videoFileName);
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
            return _fileManagerLocal.SaveTemporaryFile(video);
        }
        
        [HttpDelete("{videoFileName}")]
        public IActionResult DeleteTemporaryVideo(string videoFileName)
        {
            
            if (!_fileManagerLocal.IsTemporaryFile(videoFileName))
            {
                return BadRequest();
            }

            
            
            if (!_fileManagerLocal.FileExists(videoFileName))
            {
                return NoContent();
            }
            
            _fileManagerLocal.DeleteFile(videoFileName);
            
            return Ok();
        }
    }
}