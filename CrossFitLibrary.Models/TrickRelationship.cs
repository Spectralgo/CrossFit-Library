namespace CrossFitLibrary.Models
{
    public class TrickRelationship
    {
        public Trick Prerequisite { get; set; }
        public int PrerequisiteVersion { get; set; }
        public string PrerequisiteId { get; set; }
        public Trick Progression { get; set; }
        public int ProgressionVersion { get; set; }
        public string ProgressionId { get; set; }
    }
}