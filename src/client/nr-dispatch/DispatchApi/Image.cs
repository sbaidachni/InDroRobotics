using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace DispatchApi
{
	public class Image
	{
		string id;
		string createdAt;
        string updatedAt;
        string version;
		bool deleted;
        string uri;
        string thumbnailUri;
        Double latitude;
        Double longitude;
        string droneId;

		[JsonProperty(PropertyName = "id")]
		public string Id
		{
			get { return id; }
			set { id = value;}
		}

        [JsonProperty(PropertyName = "droneId")]
        public string DroneId
        {
            get { return droneId; }
            set { droneId = value; }
        }

        [JsonProperty(PropertyName = "createdAt")]
		public string CreatedAt
		{
			get { return createdAt; }
			set { createdAt = value;}
		}

		[JsonProperty(PropertyName = "updatedAt")]
		public string UpdatedAt
		{
			get { return updatedAt; }
			set { updatedAt = value;}
		}

        [Version]
        public string Version { get; set; }


        [JsonProperty(PropertyName = "deleted")]
        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }

        [JsonProperty(PropertyName = "uri")]
        public string Uri
        {
            get { return uri; }
            set { uri = value; }
        }

        [JsonProperty(PropertyName = "thumbnailUri")]
        public string ThumbnailUri
        {
            get { return thumbnailUri; }
            set { thumbnailUri = value; }
        }
        [JsonProperty(PropertyName = "latitude")]
        public Double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        [JsonProperty(PropertyName = "longitude")]
        public Double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

    }
}

