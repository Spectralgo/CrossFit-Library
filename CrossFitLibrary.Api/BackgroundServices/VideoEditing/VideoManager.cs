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
        private const string ThumbnailPrefix = "t";
        private readonly IWebHostEnvironment _env;
        

        public VideoManager(IWebHostEnvironment env)
        {
            _env = env;
        }

        private string WorkingDirectory => _env.WebRootPath;
        public string GetFFmpegPath => Path.Combine(_env.ContentRootPath, "ffmpeg", "ffmpeg.exe");
        

        public bool IsTemporaryFile(string fileName)
        {
            return fileName.StartsWith(TempPrefix);
        }

        public bool FileExists(string fileName)
        {
            var filePath = TemporarySavePath(fileName);

            return File.Exists(filePath);
        }

        public void DeleteFile(string fileName)
        {
            var filePath = TemporarySavePath(fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

        }

        public string DevVideoPath(string videoFileName)
        {
            return _env.IsProduction() ? null : Path.Combine(WorkingDirectory, videoFileName);
        }

        public string GenerateConvertedVideoFileName()
        {
            return $"{ConvertedPrefix}{DateTime.Now.Ticks}.mp4";
        }
        
        public string GenerateThumbnailFileName(string videoFileName)
        {
            videoFileName = videoFileName.Replace(".mp4", ".jpg");
            // slice out the first letter of the video file name to get ride of the converted prefix
            return $"{ThumbnailPrefix}{videoFileName.Substring(1)}";
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