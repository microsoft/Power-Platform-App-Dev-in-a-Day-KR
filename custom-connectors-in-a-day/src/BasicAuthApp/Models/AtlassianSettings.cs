namespace BasicAuthApp.Models
{
    public class AtlassianSettings
    {
        public const string Name = "Atlassian";

        public virtual string InstanceName { get; set; } = string.Empty;
        public virtual string Endpoint { get; set; } = string.Empty;
    }
}