#r "System.Drawing"

using System;
using System.Drawing;
using ImageProcessor;

public static void Run(Stream inputImage, Stream outputImage, TraceWriter log)
{
    log.Info($"Function triggered!!");
    
    using (var imageFactory = new ImageFactory())
    {
        imageFactory
            .Load(inputImage)
            .Resize(new Size(100, 100))
            .Save(outputImage);
    }
}