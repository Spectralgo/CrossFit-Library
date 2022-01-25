namespace CrossFitLibrary.Models.Abstractions;

public abstract class SlugModel : VersionedModel
{
    public string Slug { get; set; }
}