using CrossFitLibrary.Models.Abstractions;

namespace CrossFitLibrary.Models
{
    public class TrickCategory 
    {
        public int TrickVersion { get; set; }
        public int CategoryVersion { get; set; }
        public string TrickId { get; set; }
        public Trick Trick { get; set; }
        public string CategoryId { get; set; }
        public Category Category { get; set; }
    }
};
