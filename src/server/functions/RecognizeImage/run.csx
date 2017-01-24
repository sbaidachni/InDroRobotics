#r "System.Data"
#r "Iris.SDK.Evaluation.dll"

using System;
using System.Text;
using System.Data.SqlClient;

public static void Run(string myEventHubMessage, TraceWriter log)
{
    var connection = new SqlConnection("Server=tcp:indro.database.windows.net,1433;Initial Catalog=dispatchDb;Persist Security Info=False;User ID=troy;Password=IndroRobotics1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    connection.Open();

    var queryString = @"select count(*) from drones";
    var command = new SqlCommand(queryString, connection);
    var result = command.ExecuteScalar();

    log.Info("+++++++++++++++");
    log.Info($"C# Event Hub trigger function processed a message: {connection.ConnectionString}");
    log.Info(myEventHubMessage);
    log.Info($"{queryString} : {result}");
    log.Info("+++++++++++++++");

    connection.Close();
}