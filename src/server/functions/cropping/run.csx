#r "System.Drawing"
#r "ImageProcessor"

using System;
using System.Drawing;
using ImageProcessor;

public static void Run(Stream inStream, Stream outStream, TraceWriter log)
{
    using (var imageFactory = new ImageFactory())
    {
        imageFactory
            .Load(inStream)
            .Resize(new Size(100, 100))
            .Save(outStream);
    }
}