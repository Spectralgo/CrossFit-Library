using System.Collections.Generic;
using CrossFitLibrary.Models;

namespace CrossFitLibrary.Api.Form
{
    public class TrickForm
    {
        public string TrickName { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public IEnumerable<string> Categories { get; set; }
        // public IEnumerable<string> prerequisites { get; set; }
        // public IEnumerable<string> progressions { get; set; }

    }
}