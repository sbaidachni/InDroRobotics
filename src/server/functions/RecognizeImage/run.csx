#r "System.Data"

using System;
using System.Text;
using System.Data.SqlClient;

public static void Run(string myEventHubMessage, TraceWriter log)
{
    log.Info("Foo");
    var n = new IoTMessage();
    SqlConnection conn = new SqlConnection("Server=tcp:indro.database.windows.net,1433;Initial Catalog=dispatchDb;Persist Security Info=False;User ID=troy;Password=IndroRobotics1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
    conn.Open();
    log.Info($"C# Event Hub trigger function processed a message: {conn.ConnectionString}");
    conn.Close();
}


class IoTMessage
{
    public string test;
}