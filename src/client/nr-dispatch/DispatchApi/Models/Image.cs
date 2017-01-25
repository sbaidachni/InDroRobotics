using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace DispatchApi
{
    [JsonObject]
	public class images
	{
		[JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "droneId")]
        public string DroneId { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
		public string CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
		public string UpdatedAt { get; set; }

        [Version]
        public string Version { get; set; }
        
        [JsonProperty(PropertyName = "deleted")]
        public bool Deleted { get; set; }

        [JsonProperty(PropertyName = "uri")]
        public string Uri { get; set; }

        [JsonProperty(PropertyName = "thumbnailUri")]
        public string ThumbnailUri { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public Double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public Double Longitude { get; set; }

    }
}

