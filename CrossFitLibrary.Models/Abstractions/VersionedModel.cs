using System;

namespace CrossFitLibrary.Models.Abstractions;

public abstract class VersionedModel : PersistentModel
{
    public int Version { get; set; }
    public bool Active { get; set; }
    
    // TimeStamp on activation (date of validation)
    public DateTime TimeStamp { get; set; }
}