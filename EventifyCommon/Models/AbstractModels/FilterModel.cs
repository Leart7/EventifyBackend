namespace EventifyCommon.Models.AbstractModels
{
    public class FilterModel : BaseModel
    {
        public string Name { get; set; }
        public virtual ICollection<Event>? Events { get; set; }
    }
}
