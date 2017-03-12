using Newtonsoft.Json;

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
