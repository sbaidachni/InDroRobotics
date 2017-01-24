using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageToCloudService
{
    public class IoTMessage
    {
        [JsonProperty("DeviceName")]
        public string deviceName;

        [JsonProperty("BlobURI")]
        public string blobURI;

        [JsonProperty("Latitude")]
        public double latitude;

        [JsonProperty("Longitude")]
        public double longitude;

    }
}
