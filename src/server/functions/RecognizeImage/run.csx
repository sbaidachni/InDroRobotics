#r "System.Data"
#r "Iris.SDK.Evaluation.dll"

using System.Data.SqlClient;
using Iris;

public static void Run(string myEventHubMessage, TraceWriter log)
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