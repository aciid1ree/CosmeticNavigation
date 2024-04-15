namespace ComponentsStore.DataAccess.Entities
{
    public class ComponentEntity
    {
        public Guid id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
    }
}
