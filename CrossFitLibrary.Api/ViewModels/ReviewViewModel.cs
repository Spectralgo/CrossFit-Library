using System;
using System.Linq.Expressions;
using CrossFitLibrary.Models;
using CrossFitLibrary.Models.Moderation;

namespace CrossFitLibrary.Api.ViewModels
{
    public class ReviewViewModel
    {
        public static readonly Func<Review, object> Create = Projection.Compile();
        
        public static Expression<Func<Review, object>> Projection => 
            review => new
        {
            review.Id,
            review.ModerationItemId,
            review.Comment,
            review.Status
        };
    }
}
