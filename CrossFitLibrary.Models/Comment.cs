﻿using System.Collections.Generic;
using CrossFitLibrary.Models.Moderation;

namespace CrossFitLibrary.Models
{
    public class Comment: BaseModel<int>
    {
        public string Content { get; set; }
        public string HtmlContent { get; set; }
        
        public int? ModerationItemId { get; set; }
        public ModerationItem ModerationItem { get; set; }
        
        public int? ParentId { get; set; }
        public Comment Parent { get; set; }

        public IList<Comment> Replies { get; set; } = new List<Comment>();
        
    }
}