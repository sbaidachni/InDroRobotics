#r "System.Data"
#r "Iris.SDK.Evaluation.dll"

using System.Data.SqlClient;
using Iris;

public static void Run(ImageMessage myEventHubMessage, TraceWriter log)
{
    var connection = new SqlConnection("Server=tcp:indro.database.windows.net,1433;Initial Catalog=dispatchDb;Persist Security Info=False;User ID=troy;Password=IndroRobotics1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    connection.Open();

    var insertImages = new SqlCommand("insert into images (uri) values (@uri)", connection);

    insertImages.Parameters.AddRange(new SqlParameter[]
    {
        new SqlParameter("uri", "Foo")
    });

    insertImages.ExecuteNonQuery();

    log.Info("[Debug]");
    log.Info("+++++++++++++++");

    var imageCountQuery = @"select count(*) from images";
    var imageCountCmd = new SqlCommand(imageCountQuery, connection);
    var imageCountResult = imageCountCmd.ExecuteScalar();
    log.Info($"{nameof(imageCountQuery)} : {imageCountResult}");
    log.Info("---");
    log.Info($"{nameof(myEventHubMessage)}: {myEventHubMessage}");
    log.Info("+++++++++++++++");

    connection.Close();
}


/*
func run RecognizeImage -c "{'DeviceName':'drone1','BlobURI':'https://indrostorage.blob.core.windows.net/images/drone1_2017_1_24_13_43_24.png','Latitude':48.877199999999995,'Longitude':-123.4228}"
*/
public class ImageMessage
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