using CrossFitLibrary.Models.Abstractions;

namespace CrossFitLibrary.Models
{
    public class TrickCategory 
    {
        public Trick Trick { get; set; }
        public int TrickId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
};
