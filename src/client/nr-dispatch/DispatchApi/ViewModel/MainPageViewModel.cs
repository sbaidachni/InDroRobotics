using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms;

namespace DispatchApi
{

    public class MainPageViewModel
    {
        public ObservableCollection<MainPageItem> Items { get; set; }

        public MainPageViewModel()
        {
            Items = new ObservableCollection<MainPageItem>();
        }
    }

    public class MainPageItem
    { 

        public string DroneName
        { get; set; }

        public double Lon
        { get; set; }

        public double Lat
        { get; set; }

        public string ImageUri
        {
            get; set;
        }

        private ImageSource _Image;

        public ImageSource Image
        {
            get
            {
                if ((_Image==null)&&(ImageUri!=null))
                {
                    var storageAccount = new CloudStorageAccount(new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials("indrostorage", ""),true);
                    var blob=storageAccount.CreateCloudBlobClient();
                    var blobImage=blob.GetBlobReferenceFromServerAsync(new Uri(ImageUri));
                    blobImage.Wait();
                    Stream target=new MemoryStream();
                    blobImage.Result.DownloadToStreamAsync(target).Wait();

                    _Image=StreamImageSource.FromStream(()=>target);

                }

                return _Image;
            }
=
        }


        public ObservableCollection<AccuracyData> Items
        { get; set; }

        public string AccuracyString
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                foreach(var item in Items)
                {
                    sb.Append($"{item.ObjectName}:{item.Accuracy};");
                }

                return sb.ToString();
            }
        }
    }

    public class AccuracyData
    {
        public string ObjectName
        { get; set; }

        public double Accuracy
        { get; set; }
    }
}
