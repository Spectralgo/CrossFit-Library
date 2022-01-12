using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CrossFitLibrary.Api.BackgroundServices.VideoEditing;

public interface IFileManager
{
    string TemporarySavePath(string fileName);
    string GetFFmpegPath();
    bool FileExists(string outputFileName);
    void DeleteFile(string fileName);
    string GetSavePath(string fileName);
    Task<string> SaveTemporaryFile(IFormFile file);
    bool IsTemporaryFile(string fileName);
}