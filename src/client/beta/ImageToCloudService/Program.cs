using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ImageToCloudService
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length!=1)
            {
                Console.WriteLine("You have to pass a directory as a parameter");
                return;
            }

            string path = args[0];


            if (!Directory.Exists(path))
            {
                Console.WriteLine("Directory doesn't exist");
                return;
            }

            while (true)
            {
                
            }
        }
    }
}
