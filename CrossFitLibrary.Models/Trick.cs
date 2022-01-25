using System.Collections.Generic;
using CrossFitLibrary.Models.Abstractions;

namespace CrossFitLibrary.Models
{
    public class Trick : SlugModel, ICommentable
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }

        public IList<TrickRelationship> Prerequisites { get; set; }
        public IList<TrickRelationship> Progressions { get; set; }
        public IList<TrickCategory> TrickCategories { get; set; }
        public IList<Comment> Comments { get; set; } = new List<Comment>();
    }
}