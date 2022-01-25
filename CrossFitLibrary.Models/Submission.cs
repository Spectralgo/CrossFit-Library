using System.Collections.Generic;
using System.Security.Principal;
using CrossFitLibrary.Models.Abstractions;

namespace CrossFitLibrary.Models
{
    public class Submission : VersionedModel, ICommentable
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string TrickId { get; set; }
        public int VideoId { get; set; }
        public Video Video { get; set; }
        public bool VideoProcessed { get; set; }
        public string Description { get; set; }
        public IList<Comment> Comments { get; set; } = new List<Comment>();
    }
}