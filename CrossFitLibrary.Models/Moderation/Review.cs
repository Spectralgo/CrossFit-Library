using CrossFitLibrary.Models.Abstractions;

namespace CrossFitLibrary.Models.Moderation
{
    public class Review : PersistentModel
    {
        public int ModerationItemId { get; set; }
        public ModerationItem ModerationItem { get; set; }
        
        public string Comment { get; set; }
        public ReviewStatus Status { get; set; }
    }

    public enum ReviewStatus
    {
        Approved = 0,
        Rejected = 1,
        Waiting = 2,
    }
}