#r "System.Data"
#r "Iris.SDK.Evaluation.dll"
#r "Microsoft.Rest.ClientRuntime.dll"

// 8.0.1 for net45
#r "Microsoft.WindowsAzure.Storage.dll"

// 2.0.18 for net45
#r "King.Azure.dll"

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Iris;
using Iris.SDK;
using Iris.SDK.Models;
using Iris.SDK.Evaluation;
using System.Net.Http;
using System.Net.Http.Headers;

private static string ConnString = "";
private static string ImageRepository = "";

public static async void Run(EventHubMessage eventHubMessage, TraceWriter log)
{
    ConnString = System.Environment.GetEnvironmentVariable("ConnString", EnvironmentVariableTarget.Process);
    ImageRepository = System.Environment.GetEnvironmentVariable("ImageRepository", EnvironmentVariableTarget.Process);

    var container = new King.Azure.Data.Container("images", ImageRepository);

    var lastIndex = eventHubMessage.BlobURI.LastIndexOf("/") + 1;
    var blogUri = eventHubMessage.BlobURI.Substring(
        lastIndex,
        eventHubMessage.BlobURI.Length - lastIndex);

    log.Info($"{nameof(blogUri)}: {blogUri}");

    var image = container.Get(blogUri).Result;

    using (var connection = new SqlConnection(ConnString))
    {
        connection.Open();

        var imageId = InsertImagesIntoDb(connection, eventHubMessage, log);
        var list=ReadIrisMetadataFromDb(connection, log);
        foreach(var p in list) 
        {
            log.Info($"{nameof(IrisMetadata.Uri)}: {p.ProjectId}");

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Prediction-Key", p.ProjectId);
                string body=$"{{\"Url\": \"{eventHubMessage.BlobURI}\"}";
                byte[] byteData = Encoding.UTF8.GetBytes(body);

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var t=await client.PostAsync(p.Uri, content);   
                    var strRes = await t.Content.ReadAsStringAsync();
                    log.Info(strRes);  
                 /*
                var irisResult = endpoint.EvaluateImage(stream);
                irisResult.Classifications.ToList().ForEach(evaluation =>
                {
                    log.Info($"{nameof(evaluation.ClassProperty)} {evaluation.ClassProperty}");
                    log.Info($"{nameof(evaluation.Probability)} {evaluation.Probability}");
                    log.Info($"{nameof(evaluation.GetType)} {evaluation.GetType()}");

                    if (evaluation.ClassProperty != "Other")
                    {
                        InsertIrisEvaluationIntoDb(connection, imageId, evaluation, log);
                    }
                });*/       
                }
            }
            catch(Exception ex)
            {
                log.Info(ex.InnerException.StackTrace);
            }
        }
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

private static void InsertIrisEvaluationIntoDb(
        SqlConnection connection,
        string imageId,
        ImageClassEvaluation imageClassEvaluation,
        TraceWriter log)
{
    const string query = @"
        insert into iris_images (objectName, accuracy, imageId) 
        values (@objectName, @accuracy, @imageId)";

    var insertIrisEval = new SqlCommand(query, connection);

    insertIrisEval.Parameters.AddRange(new SqlParameter[]
    {
        new SqlParameter("imageId", imageId),
        new SqlParameter("objectName", imageClassEvaluation.ClassProperty),
        new SqlParameter("accuracy", imageClassEvaluation.Probability)});

    var result = insertIrisEval.ExecuteNonQuery();

    log.Info($"{query}: {result}");
}

private static string InsertImagesIntoDb(
        SqlConnection connection,
        EventHubMessage eventHubMessage,
        TraceWriter log)
{
    // todo: Check concurrency problems. :) 
    const string query = @"
insert into images (uri, latitude, longitude, droneId) 
values (@uri, @latitude, @longitude, @droneId);
select top(1) id from images order by createdAt desc";

    var insertImages = new SqlCommand(query, connection);

    insertImages.Parameters.AddRange(new SqlParameter[]
    {
        new SqlParameter("uri", eventHubMessage.BlobURI),
        new SqlParameter("latitude", eventHubMessage.Latitude),
        new SqlParameter("longitude", eventHubMessage.Longitude),
        new SqlParameter("droneId", eventHubMessage.DeviceName)
    });

    var imageId = insertImages.ExecuteScalar().ToString();

    log.Info($"{query}: {imageId}");

    return imageId;
}

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