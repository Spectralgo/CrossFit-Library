using System.Collections.Generic;

namespace CrossFitLibrary.Models
{
    public interface ICommentable
    {
        IList<Comment> Comments { get; set; }
    }
}