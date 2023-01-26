using System.Collections.Generic;

using Newtonsoft.Json;

namespace AuthCodeAuthApp.Models
{
    public class GraphUser
    {
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        public string Id { get; set; }
        public List<string> BusinessPhones { get; set; } = new List<string>();
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string JobTitle { get; set; }

        [JsonProperty("mail")]
        public string Email { get; set; }

        public object MobilePhone { get; set; }
        public string OfficeLocation { get; set; }
        public object PreferredLanguage { get; set; }
        public string UserPrincipalName { get; set; }
    }
}