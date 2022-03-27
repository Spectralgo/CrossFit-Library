using System;
using System.Linq;
using System.Linq.Expressions;
using CrossFitLibrary.Models;
using CrossFitLibrary.Models.Moderation;

namespace CrossFitLibrary.Api.ViewModels
{
    public static class ModerationItemViewModels
    {
        public static readonly Func<ModerationItem, object> Create = Projection.Compile();
        public static Expression<Func<ModerationItem, object>> Projection => 
            modItem => new
        {
            modItem.Id,
            modItem.Current,
            modItem.Target,
            modItem.Type,
            Comments = modItem.Comments.AsQueryable().Select(CommentViewModel.Projection).ToList(),
            Review = modItem.Reviews.AsQueryable().Select(ReviewViewModel.Projection).ToList(),
        };
    }
}