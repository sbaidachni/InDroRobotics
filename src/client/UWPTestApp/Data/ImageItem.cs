using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UWPTestApp.Data
{
    public class ImageItem
    {
        public string ImageUri { get; set; }

        public string ObjectProbabilityString { get; set; }
    }

    public class Utility
    {
        public static List<ImageItem> data;

        public async static Task<List<ImageItem>> DownloadItems()
        {
            List<ImageItem> list = new List<ImageItem>();
            MobileServiceClient MobileService = new MobileServiceClient("http://indrobuildmobile.azurewebsites.net");
            var imTable = MobileService.GetTable<images>();
            var items = await imTable.ToEnumerableAsync();

            foreach(var item in items)
            {
                string accResult = "";
                var r = await (from a in MobileService.GetTable<iris_images>() where a.imageId==item.id select a).ToEnumerableAsync();
                foreach(var i in r)
                {
                    accResult = accResult + $"{i.objectName}: {i.accuracy}";
                }
                if (accResult == "") accResult = "No known objects";
                list.Add(new ImageItem() { ImageUri = item.uri, ObjectProbabilityString = accResult });
            }

           


            return list;
        }
    }

    public class images
    {
        public string id { get; set; }

        public string uri { get; set; }
    }

    public class iris_images
    {
        public string id { get; set; }

        public string objectName { get; set; }

        public double accuracy { get; set; }

        public string imageId { get; set; }
    }

}
