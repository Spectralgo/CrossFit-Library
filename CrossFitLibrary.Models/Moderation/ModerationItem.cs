using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace CrossFitLibrary.Models.Moderation
{
    public class ModerationItem: BaseModel<int>
    {
        // from BaseModel
        // int Id  
        // bool Deleted

        public string Target { get; set; }
        public string Type { get; set; }
        public IList<Comment> Comments { get; set; } = new List<Comment>();
    }
}