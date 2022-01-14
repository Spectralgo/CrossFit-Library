namespace CrossFitLibrary.Models
{
    public class Video : BaseModel<int>
    {
        
        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}