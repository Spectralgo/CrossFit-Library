namespace CrossFitLibrary.Models.Abstractions;

public abstract class PersistentModel
{
    public int Id { get; set; }
    public bool Deleted { get; set; }
}