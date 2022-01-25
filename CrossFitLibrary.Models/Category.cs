using System.Collections.Generic;
using CrossFitLibrary.Models.Abstractions;

namespace CrossFitLibrary.Models
{
    public class Category : SlugModel
    {
        
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<TrickCategory> Tricks { get; set; }
    }
}