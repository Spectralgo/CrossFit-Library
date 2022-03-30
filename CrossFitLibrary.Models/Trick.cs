using System.Collections.Generic;
using CrossFitLibrary.Models.Abstractions;

namespace CrossFitLibrary.Models
{
    public class Trick : VersionedModel, ICommentable
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }

        public IList<TrickRelationship> Prerequisites { get; set; } = new List<TrickRelationship>();
        public IList<TrickRelationship> Progressions { get; set; } = new List<TrickRelationship>();
        public IList<TrickCategory> TrickCategories { get; set; } = new List<TrickCategory>();
        public IList<Comment> Comments { get; set; } = new List<Comment>();
    }
}