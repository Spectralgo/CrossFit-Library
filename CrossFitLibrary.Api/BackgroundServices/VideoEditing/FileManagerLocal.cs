using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace CrossFitLibrary.Api.BackgroundServices.VideoEditing
{
    // Todo: Chose to make a FileManager or an ImageManager but do not let images processing live here
    public class FileManagerLocal : IFileManager
    {
        private readonly IWebHostEnvironment _env;
        

        public FileManagerLocal(IWebHostEnvironment env)
        {
            _env = env;
        }

        private static string TempPrefix => TrickingLibraryConstants.Files.TempPrefix;
        private string WorkingDirectory => _env.WebRootPath;
        public string GetFFmpegPath() => Path.Combine(_env.ContentRootPath, "ffmpeg", "ffmpeg.exe");
        

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

        public string GetSavePath(string fileName)
        {
            return _env.IsProduction() ? null : Path.Combine(WorkingDirectory, fileName);
        }



        public async Task<string> SaveTemporaryFile(IFormFile file)
        {
            var videoTemporaryFileName =
                string.Concat(TempPrefix, DateTime.Now.Ticks, Path.GetExtension(file.FileName));
            var savePath = TemporarySavePath(videoTemporaryFileName);

            await using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(fileStream);
            }

            return videoTemporaryFileName;
        }

        public string TemporarySavePath(string fileName)
        {
            return Path.Combine(WorkingDirectory, fileName);
        }
    }
}