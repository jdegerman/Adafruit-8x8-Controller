using Adafruit8x8Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctl = new Controller())
            {
                ctl.SetPixel(0, 0, true);
                Console.Read();

            }
        }

    }
}
