using CrossFitLibrary.Models.Moderation;

namespace CrossFitLibrary.Api.Form;

public class ReviewForm
{
        public string Comment { get; set; }
        public ReviewStatus Status { get; set; }
}