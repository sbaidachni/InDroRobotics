using System;
using System.IO;

public static void Run(Stream original, string name, TraceWriter log)
{
    log.Info("hi hi?");
    log.Info(name);
    log.Info("bye bye?");
}