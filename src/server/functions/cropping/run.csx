#r "System.Drawing"
#r "ImageProcessor"

using System;
using System.Drawing;
using ImageProcessor;

public static void Run(Stream inStream, Stream resized, TraceWriter log)
{
    using (var imageFactory = new ImageFactory())
    {
        imageFactory
            .Load(original)
            .Resize(new Size(100, 100))
            .Save(resized);
    }
}