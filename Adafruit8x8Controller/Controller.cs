using System.Diagnostics;

namespace Adafruit8x8Controller
{
    public class Controller : ControllerBase
    {
        ControllerBase controller;
        public Controller(int addr = 0x70, DisplayUpdateMode updateMode = DisplayUpdateMode.Automatic)
        {
            if(Debugger.IsAttached)
            {
                controller = new ConsoleController();
            }
            else
            {
                controller = new AdafruitController(addr, updateMode);
            }
        }
               
        public override void Dispose() => controller.Dispose();

        public override void SetBlink(Blink blink) => controller.SetBlink(blink);

        public override void SetBrightness(byte brightness) => controller.SetBrightness(brightness);
               
        public override void Update() => controller.Update();
    }
}
