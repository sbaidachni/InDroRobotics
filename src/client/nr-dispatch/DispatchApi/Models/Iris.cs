using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchApi
{
    
    public class Iris
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public string UpdatedAt { get; set; }

        [Version]
        public string Version { get; set; }

        [JsonProperty(PropertyName = "deleted")]
        public bool Deleted { get; set; }

        [JsonProperty(PropertyName = "projectId")]
        public string projectId { get; set; }

        [JsonProperty(PropertyName = "objectName")]
        public string objectName { get; set; }

        [JsonProperty(PropertyName = "irisUri")]
        public string IrisUri { get; set; }

    }
}
