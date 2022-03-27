using CrossFitLibrary.Models.Abstractions;

namespace CrossFitLibrary.Models
{
    public class Video : VersionedModel
    {
        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}