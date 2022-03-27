using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.Principal;
using CrossFitLibrary.Models.Abstractions;
using CrossFitLibrary.Models.Moderation;

namespace CrossFitLibrary.Models
{
    public class Comment: VersionedModel
    {
        public string Content { get; set; }
        public string HtmlContent { get; set; }
        public int Likes { get; set; } // Todo: implement likes in the comment component

        public string DateOfCreation { get; set; }  
        // I'm using this format, when I add it form the Comment controller:
        // DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
        
        public int? ModerationItemId { get; set; } // Target(where) , Type(what)
        
        public ModerationItem ModerationItem { get; set; } 
        
        public int? ParentId { get; set; }
        public Comment Parent { get; set; }

        public IList<Comment> Replies { get; set; } = new List<Comment>();
        public string? TrickId { get; set; }
        public int? SubmissionId { get; set; }
    }
}