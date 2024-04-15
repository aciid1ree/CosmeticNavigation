namespace CosmeticsStore.Models
{
    public class MainText
    {
        public string text { get; set; }
        public IEnumerable<string> GetArray()
        {
            IEnumerable<string> words = text.Split(new char[] { ' ', ',', '!', '\n', '?', '\t' }, StringSplitOptions.RemoveEmptyEntries).Select(e => e.ToLower());
            return words;
        }
    }
}
