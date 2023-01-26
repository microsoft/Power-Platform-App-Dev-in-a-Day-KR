using Newtonsoft.Json;

namespace BasicAuthApp.Models
{
    public class AtlassianUser
    {
        public virtual string Type { get; set; }
        public virtual string AccountId { get; set; }
        public virtual string AccountType { get; set; }
        public virtual string Email { get; set; }
        public virtual string PublicName { get; set; }
        public virtual Profilepicture ProfilePicture { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual bool IsExternalCollaborator { get; set; }

        [JsonProperty("_expandable")]
        public virtual Expandable Expandable { get; set; }

        [JsonProperty("_links")]
        public virtual Links Links { get; set; }
    }

    public class Profilepicture
    {
        public virtual string Path { get; set; }
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public virtual bool IsDefault { get; set; }
    }

    public class Expandable
    {
        public virtual string Operations { get; set; }
        public virtual string PersonalSpace { get; set; }
    }

    public class Links
    {
        public virtual string Self { get; set; }

        [JsonProperty("_base")]
        public virtual string Base { get; set; }

        public virtual string Context { get; set; }
    }
}