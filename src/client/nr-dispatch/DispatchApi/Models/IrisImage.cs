using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispatchApi
{
    public class IrisImage
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

        [JsonProperty(PropertyName = "imageId")]
        public string ImageId { get; set; }

        [JsonProperty(PropertyName = "accuracy")]
        public Double Accuracy { get; set; }

        [JsonProperty(PropertyName = "objectName")]
        public string ObjectName { get; set; }
        

    }
}
