using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adafruit8x8Controller
{
    internal class ConsoleController : ControllerBase
    {
        public ConsoleController()
        {
            CreateBuffers();
        }
        public override void Dispose()
        {
            
        }

        public override void SetBlink(Blink blink)
        {
            
        }

        public override void SetBrightness(byte brightness)
        {
            
        }

        public override void Update()
        {
            Console.WriteLine("Foo!");
        }
    }
}
