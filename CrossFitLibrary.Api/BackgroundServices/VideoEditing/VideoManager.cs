using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace CrossFitLibrary.Api.BackgroundServices
{
    public class VideoManager
    {
        private const string TempPrefix = "temp_";
        private const string ConvertedPrefix = "c";
        private readonly IWebHostEnvironment _env;

        public VideoManager(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string WorkingDirectory => _env.WebRootPath;

        public bool IsTemporaryFile(string fileName)
        {
            return fileName.StartsWith(TempPrefix);
        }

        public bool TemporaryVideoExists(string videoFileName)
        {
            var videoFilePath = TemporarySavePath(videoFileName);

            return File.Exists(videoFilePath);
        }

        public void DeleteTemporaryVideo(string videoFileName)
        {
            var videoFilePath = TemporarySavePath(videoFileName);

            File.Delete(videoFilePath);
        }

        public string DevVideoPath(string videoFileName)
        {
            return _env.IsProduction() ? null : Path.Combine(WorkingDirectory, videoFileName);
        }

        public string GenerateConvertedVideoFileName()
        {
            return $"{ConvertedPrefix}{DateTime.Now.Ticks}.mp4";
        }

        public async Task<string> SaveTemporaryVideo(IFormFile video)
        {
            var videoTemporaryFileName =
                string.Concat(TempPrefix, DateTime.Now.Ticks, Path.GetExtension(video.FileName));
            var savePath = TemporarySavePath(videoTemporaryFileName);

            await using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                await video.CopyToAsync(fileStream);
            }

            return videoTemporaryFileName;
        }

        public string TemporarySavePath(string videoFileName)
        {
            return Path.Combine(WorkingDirectory, videoFileName);
        }
    }
}