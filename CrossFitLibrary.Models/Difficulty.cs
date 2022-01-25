using System.Collections.Generic;
using CrossFitLibrary.Models.Abstractions;

namespace CrossFitLibrary.Models
{
    public class Difficulty : SlugModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<Trick> Tricks { get; set; }
    }
}