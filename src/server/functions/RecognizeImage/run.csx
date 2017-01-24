#r "System.Data"
#r "Iris.SDK.Evalutation.dll"

using System.Data.SqlClient;

public static void Run(string myEventHubMessage, TraceWriter log)
{
    log.Info("Foo");

    var n = new IoTMessage();
    SqlConnection conn = new SqlConnection("Server=tcp:indro.database.windows.net,1433;Initial Catalog=dispatchDb;Persist Security Info=False;User ID=troy;Password=IndroRobotics1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    

    log.Info("+++++++++++++++");
    log.Info($"C# Event Hub trigger function processed a message: {connection.ConnectionString}");
    log.Info(myEventHubMessage);
    log.Info($"{queryString} : {result}");
    log.Info("+++++++++++++++");

    conn.Open();
    log.Info($"C# Event Hub trigger function processed a message: {conn.ConnectionString}");
    conn.Close();
}


class IoTMessage
{
    public string test;
}