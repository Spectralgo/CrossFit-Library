using System.Collections.Generic;
using CrossFitLibrary.Models;

namespace CrossFitLibrary.Api.Form
{
    public class TrickForm
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<int> Prerequisites { get; set; }
        public IEnumerable<int> Progressions { get; set; }

    }
}