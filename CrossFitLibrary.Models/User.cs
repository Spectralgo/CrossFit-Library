using System.Collections;
using System.Collections.Generic;
using CrossFitLibrary.Models.Abstractions;

namespace CrossFitLibrary.Models;

public class User : BaseModel<string>
{
    public string Username { get; set; }
    public string ImageUrl { get; set; }

    public IList<Submission> Submissions { get; set; } = new List<Submission>();

}