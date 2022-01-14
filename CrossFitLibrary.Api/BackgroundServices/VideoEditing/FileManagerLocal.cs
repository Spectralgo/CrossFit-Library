using System;
using System.IO;
using System.Threading.Tasks;
using CrossFitLibrary.Api.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace CrossFitLibrary.Api.BackgroundServices.VideoEditing
{
    // Todo: Chose to make a FileManager or an ImageManager but do not let images processing live here
    public class FileManagerLocal : IFileManager
    {
        private readonly IOptionsMonitor<FileSettings> _fileSettingsMonitor;
        private readonly IWebHostEnvironment _env;
        

        public FileManagerLocal(
            IOptionsMonitor<FileSettings> fileSettingsMonitor,
            IWebHostEnvironment env)
        {
            _fileSettingsMonitor = fileSettingsMonitor;
            _env = env;
        }

        private static string TempPrefix => CrossFitLibraryConstants.Files.TempPrefix;
        private string WorkingDirectory => _env.WebRootPath;
        public string GetFFmpegPath() => Path.Combine(_env.ContentRootPath, "ffmpeg", "ffmpeg.exe");
        public string GetFileUrl(string fileName, FileType fileType)
        {
            // This logic is implemented to let the program resolve the environment settings at startup (dev or prod)
            // The Urls to resolve will be determined by the provider given in the settings. (S3 or local)
            var settings = _fileSettingsMonitor.CurrentValue;
            return fileType switch
            {
                FileType.Image => $"{settings.ImageUrl}/{fileName}",
                FileType.Video => $"{settings.VideoUrl}/{fileName}",
                _ => throw new ArgumentException(nameof(fileType))
            };
        }


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
            return Path.Combine(WorkingDirectory, fileName);
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