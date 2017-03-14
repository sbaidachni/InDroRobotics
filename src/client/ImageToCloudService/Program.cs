using Microsoft.Azure.Devices.Client;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageToCloudService
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
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
                storageAccount = new CloudStorageAccount(new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials("indrobuildstorage", ConfigurationManager.AppSettings["azureKey1"]), true);

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

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }



            DirectoryInfo dirImages = new DirectoryInfo(args[0]);
            while (true)
            {
                var files = dirImages.GetFiles("*.jpg");
                foreach (FileInfo f in files)
                {

                    Console.WriteLine("About to upload file {0}", f.Name);

                    try
                    {
                        var cloudBlockBlob = container.GetBlockBlobReference(getBlobName(f));
                        cloudBlockBlob.UploadFromFile(f.FullName);
                        Console.WriteLine("Uploaded file {0}", f.Name);

                        Console.WriteLine("Sending message to IoT");
                        IoTMessage msg = new IoTMessage()
                        {
                            blobURI = cloudBlockBlob.StorageUri.PrimaryUri.AbsoluteUri,
                            latitude = getLatitude(f),
                            longitude = getLongitude(f),
                            deviceName = ConfigurationManager.AppSettings["deviceName"]
                        };
                        var msgString = JsonConvert.SerializeObject(msg);
                        var msgOut = new Message(Encoding.ASCII.GetBytes(msgString));
                        client.SendEventAsync(msgOut).Wait();
                        Console.WriteLine("Sent message to IoT");

                        f.Delete();

                        Thread.Sleep(15000);
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine("File in use..  skipping");
                    }


                }
            }
        }
        static string getBlobName(FileInfo f)
        {

            var blobName = String.Format($"{ConfigurationManager.AppSettings["deviceName"]}_{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}{f.Extension}");
            return blobName;
        }



        static double getLatitude(FileInfo f)
        {
            string fileName = f.Name;
            fileName = fileName.Replace(f.Extension, "");

            if (fileName.Contains(","))
            {
                try
                {
                    return Double.Parse(fileName.Split(',')[0]);
                }
                catch
                {
                    Console.WriteLine("Filename not in expected format, assigning dummy latitude");
                    return 0.0;
                }
            }
            else
            {
                return 0.0;
            }
        }

        static double getLongitude(FileInfo f)
        {
            string fileName = f.Name;
            fileName = fileName.Replace(f.Extension, "");

            if (fileName.Contains(","))
            {
                try
                {
                    return Double.Parse(fileName.Split(',')[1]);
                }
                catch
                {
                    Console.WriteLine("Filename not in expected format, assigning dummy longitude");
                    return 0.0;
                }
            }
            else
            {
                return 0.0;
            }
        }
    }
}
