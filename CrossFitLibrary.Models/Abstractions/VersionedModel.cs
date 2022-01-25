using System;

namespace CrossFitLibrary.Models.Abstractions;

public abstract class VersionedModel : PersistentModel
{
    public int Version { get; set; }
    public bool Active { get; set; }
    public DateTime TimeStamp { get; set; }
}