using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace ImageToCloudService
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length!=1)
            {
                Console.WriteLine("You have to pass a directory as a parameter");
                return;
            }

            string path = args[0];


            if (!Directory.Exists(path))
            {
                Console.WriteLine("Directory doesn't exist");
                return;
            }

            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount;
            try
            {
                storageAccount = new CloudStorageAccount(new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials("indrostorage", ConfigurationManager.AppSettings["azureKey1"]), true);

            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["imagesContainerName"]);
            container.CreateIfNotExists();

            DeviceClient client;
            try
            {
                client = DeviceClient.Create(ConfigurationManager.AppSettings["IoTHost"],
                        new DeviceAuthenticationWithRegistrySymmetricKey(ConfigurationManager.AppSettings["deviceName"], ConfigurationManager.AppSettings["deviceKey"]));

            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }



            DirectoryInfo dirImages = new DirectoryInfo(args[0]);
            while (true)
            {
                var dirs = dirImages.GetFiles("*.png");
                foreach (FileInfo f in dirs)
                {

                    Console.WriteLine("About to upload file {0}", f.Name);

                    var cloudBlockBlob = container.GetBlockBlobReference(getBlobName());
                    cloudBlockBlob.UploadFromFile(f.FullName);
                    Console.WriteLine("Uploaded file {0}", f.Name);

                    Console.WriteLine("Sending message to IoT");
                    IoTMessage msg = new IoTMessage() {blobURI= cloudBlockBlob.StorageUri.PrimaryUri.AbsoluteUri, latitude=0.0, longitude=0.0 };
                    var msgString = JsonConvert.SerializeObject(msg);
                    var msgOut = new Message(Encoding.ASCII.GetBytes(msgString));
                    client.SendEventAsync(msgOut).Wait();
                    Console.WriteLine("Sent message to IoT");

                    f.Delete();


                }
            }
        }
        static string getBlobName() {

            var blobName = String.Format($"{ConfigurationManager.AppSettings["deviceName"]}_{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}");
            return blobName;
        }
    }
}
