﻿namespace CrossFitLibrary.Models
{
    public class TrickRelationship
    {
        public Trick Prerequisite { get; set; }
        public int PrerequisiteId { get; set; }
        
        public Trick Progression { get; set; }
        public int ProgressionId { get; set; }
        public bool Active { get; set; }
    }
}