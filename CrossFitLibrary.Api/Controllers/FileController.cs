using System;
using System.IO;
using System.Threading.Tasks;
using CrossFitLibrary.Api.BackgroundServices.VideoEditing;
using CrossFitLibrary.Api.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrossFitLibrary.Api.Controllers;

[Route("api/files")]
public class FileController : ControllerBase
{
    private readonly IFileManager _fileManagerLocal;

    public FileController(IFileManager fileManagerLocal)
    {
        _fileManagerLocal = fileManagerLocal;
    }


    [HttpGet("{type}/{fileName}")]
    public IActionResult GetFile(string type, string fileName )
    {
        var mime = type.Equals(nameof(FileType.Image), StringComparison.InvariantCultureIgnoreCase)
            ? "image/jpg"
            : type.Equals(nameof(FileType.Video), StringComparison.InvariantCultureIgnoreCase)
                ? "video/mp4"
                : null;
        if (mime == null)
        {
            return BadRequest();
        }
        
        var filePath = _fileManagerLocal.GetSavePath(fileName);
        if (string.IsNullOrEmpty(filePath))
        {
            return BadRequest();
        }

        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return new FileStreamResult(fileStream, mime);
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