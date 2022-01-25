using CrossFitLibrary.Models.Abstractions;

namespace CrossFitLibrary.Models
{
    public class Video : VersionedModel
    {
        public int Id { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}