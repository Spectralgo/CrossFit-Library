namespace CrossFitLibrary.Models
{
    public class TrickCategory : BaseModel<string>
    {
        public string TrickId { get; set; }
        public Trick Trick { get; set; }
        public string CategoryId { get; set; }
        public Category Category { get; set; }
    }
};
