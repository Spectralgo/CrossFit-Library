namespace CrossFitLibrary.Models
{
    public class Video : BaseModel<int>
    {
        
        public string VideoLink { get; set; }
        public string ThumbnailLink { get; set; }
    }
}