using System.Collections;
using System.Collections.Generic;
using CrossFitLibrary.Models.Abstractions;

namespace CrossFitLibrary.Models;

public class User : VersionedModel
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string ImageUrl { get; set; }

    public IList<Submission> Submissions { get; set; } = new List<Submission>();

}