using System;
using System.IO;

public static void Run(Stream inStream, string name, TraceWriter log)
{
    log.Info("hi hi?");
    log.Info(name);
    log.Info("bye bye?");
}