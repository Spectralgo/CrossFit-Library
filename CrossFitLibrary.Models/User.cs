using System.Collections;
using System.Collections.Generic;
using CrossFitLibrary.Models.Abstractions;

namespace CrossFitLibrary.Models;

public class User : VersionedModel
{
    public new string Id { get; set; } // overriding the int id prop from the VersionModel
    public string Username { get; set; }
    public string ImageUrl { get; set; }

    public IList<Submission> Submissions { get; set; } = new List<Submission>();

}