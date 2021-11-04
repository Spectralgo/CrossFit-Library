namespace CrossFitLibrary.Models
{
    public class BaseModel<TKey>
    {
        
        public TKey Id { get; set; }
        public bool Deleted { get; set; }
    }
}