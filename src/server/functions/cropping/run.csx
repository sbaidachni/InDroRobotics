using System;
using System.IO;

public static void Run(TimerInfo myTimer, TraceWriter log)
{
    //Get Photo from Blob?
    var photoBytes = File.ReadAllBytes(file);
    // Format is automatically detected though can be changed.
    var format = new JpegFormat { Quality = 70 };
    var size = new Size(150, 0)
    using (var inStream = new MemoryStream(photoBytes))
    {
        using (var outStream = new MemoryStream()) //PUSH to Output
        {
            // Initialize the ImageFactory using the overload to preserve EXIF metadata.
            using (var imageFactory = new ImageFactory(preserveExifData:true))
            {
                // Load, resize, set the format and quality and save an image.
                imageFactory.Load(inStream)
                            .Resize(size)
                            .Format(format)
                            .Save(outStream);
            }
        }
    }
}