using static System.Net.Mime.MediaTypeNames;

namespace CosmeticsStore.Models
{
    public class Components
    {
        public Guid id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public Components(Guid id, string name, string description)
        {
            this.id = id;
            this.name = name;
            this.description = description;
        }
        public static (Components Component, string Error) Create(Guid Id,string name, string description)
        {
            var error = String.Empty;
            if (String.Empty == name)
            {
                error = "Введите поле name";
            }
            var Component = new Components(Id,name, description);
            return (Component, error);
        }
    }
}
