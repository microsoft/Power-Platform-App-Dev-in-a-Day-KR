namespace AuthCodeAuthApp.Models
{
    public class GraphSettings
    {
        public const string Name = "Graph";

        public virtual string Endpoint { get; set; } = string.Empty;
    }
}