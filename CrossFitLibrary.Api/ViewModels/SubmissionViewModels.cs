using System;
using System.Linq.Expressions;
using CrossFitLibrary.Models;

namespace CrossFitLibrary.Api.ViewModels;

public static class SubmissionViewModels
{
    public static readonly Func<Submission, object> Create = Projection.Compile();

    public static Expression<Func<Submission, object>> Projection =>
        submission => new
        {
            submission.Id,
            submission.Video.ThumbnailUrl,
            submission.Video.VideoUrl,
            User = new
            {
                submission.User.Username,
                submission.User.ImageUrl
            }
        };
}