#r "System.Data"
#r "Iris.SDK.Evaluation.dll"
#r "Microsoft.Rest.ClientRuntime.dll"
#r "Microsoft.WindowsAzure.Storage.dll"
#r "King.Azure.dll"

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Iris;
using Iris.SDK;
using Iris.SDK.Evaluation;

private const string ConnString = "Server=tcp:indro.database.windows.net,1433;Initial Catalog=dispatchDb;Persist Security Info=False;User ID=troy;Password=IndroRobotics1!;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
private const string ImageRepository = "DefaultEndpointsProtocol=https;AccountName=indrostorage;AccountKey=H+aiIp95f87SMHQr65YcwAbOq8LWqQf/wbbK8u93XB2dKBwzZvLxdW944L68+urQJYjU61lfplHnT8t8nL3UeQ==";
private const string BlobCredentials = "H+aiIp95f87SMHQr65YcwAbOq8LWqQf/wbbK8u93XB2dKBwzZvLxdW944L68+urQJYjU61lfplHnT8t8nL3UeQ==";

public static void Run(EventHubMessage eventHubMessage, TraceWriter log)
{
    var container = new King.Azure.Data.Container("Images", ImageRepository);
    var image = container.Get(eventHubMessage.BlobURI).Result;

    using (var connection = new SqlConnection(ConnString))
    {
        connection.Open();

        InsertImagesIntoDb(connection, eventHubMessage, log);
        ReadIrisMetadataFromDb(connection, log).ForEach(p =>
        {
            // Uri: https://ppevisual.cloudapp.net/projects/66f494b3-a393-47f9-8d9a-7bc9e8d1399c/performance#
            // ProjectId: 408cfa4001094d4ab15d5c2b8ce5b4d5            
            log.Info($"{nameof(IrisMetadata.Uri)}: {p.Uri}");

            var endpoint = new EvaluationEndpoint(
                new EvaluationEndpointCredentials(p.ProjectId));

            using (var stream = new System.IO.MemoryStream())
            {
                stream.Write(image, 0, image.Length);
                
                var irisResult = endpoint.EvaluateImage(image);

            // log.Info($"{nameof(irisResult)} {irisResult}");
            }
        });
    }
}

private static List<IrisMetadata> ReadIrisMetadataFromDb(
    SqlConnection connection,
    TraceWriter log)
{
    const string query = "select irisUri, objectName, projectId from iris";

    var selectIrisMetadata = new SqlCommand(query, connection);
    var reader = selectIrisMetadata.ExecuteReader();

    var projects = new List<IrisMetadata> { };
    while (reader.Read())
    {
        var metadata = new IrisMetadata
        {
            Uri = reader["irisUri"].ToString(),
            ProjectId = reader["projectId"].ToString(),
            ObjectName = reader["objectName"].ToString()
        };

        projects.Add(metadata);
    }

    return projects;
}

private static void InsertImagesIntoDb(
    SqlConnection connection,
    EventHubMessage eventHubMessage,
    TraceWriter log)
{
    const string query = @"
insert into images (uri, latitude, longitude, droneId) 
values (@uri, @latitude, @longitude, @droneId)";

    var insertImages = new SqlCommand(query, connection);

    insertImages.Parameters.AddRange(new SqlParameter[]
    {
        new SqlParameter("uri", eventHubMessage.BlobURI),
        new SqlParameter("latitude", eventHubMessage.Latitude),
        new SqlParameter("longitude", eventHubMessage.Longitude),
        new SqlParameter("droneId", eventHubMessage.DeviceName),
    });

    insertImages.ExecuteNonQuery();

    log.Info("[Debug]");
    log.Info("+++++++++++++++");

    var imageCountQuery = @"select count(*) from images";
    var imageCountCmd = new SqlCommand(imageCountQuery, connection);
    var imageCountResult = imageCountCmd.ExecuteScalar();
    log.Info($"{nameof(imageCountQuery)} : {imageCountResult}");
    log.Info("---");
    log.Info($"{nameof(eventHubMessage)}: {eventHubMessage}");
    log.Info("+++++++++++++++");
}

/*
Run this from the /functions directory to test the RecognizeImage function.

func run RecognizeImage -c "{'DeviceName':'drone1','BlobURI':'https://indrostorage.blob.core.windows.net/images/drone1_2017_1_24_13_43_24.png','Latitude':48.877199999999995,'Longitude':-123.4228}"

*/
public class EventHubMessage
{
    public string DeviceName { get; set; }

    public string BlobURI { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public override string ToString()
    {
        return $"{DeviceName} {BlobURI} {Latitude} {Longitude}";
    }
}

public class IrisMetadata
{
    public string Uri { get; set; }

    public string ProjectId { get; set; }

    public string ObjectName { get; set; }
}