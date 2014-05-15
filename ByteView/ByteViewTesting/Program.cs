using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celarix.Drawing;

namespace ByteViewTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (byte b in Celarix.Drawing.Colors.DefaultPalettes.GenerateRange(64))
            {
                Console.WriteLine(b);
            }
            Console.ReadKey();
        }
    }
}
