using Raspberry.IO.InterIntegratedCircuit;
using System.Collections.Generic;

namespace Adafruit8x8Controller
{
    internal class AdafruitController : ControllerBase
    {
        const int START_DISPLAY = 0x21;
        const int BLINK_CMD = 0x80;
        const int BLINK_DISPLAY_ON = 0x01;
        const int BRIGHTNESS_CMD = 0xE0;
        const byte BRIGHTNESS_LO = 0x0;
        const byte BRIGHTNESS_HI = 0xF;

        I2cDriver driver;
        I2cDeviceConnection connection;

        /// <summary>
        /// Constructor for the controller
        /// </summary>
        /// <param name="addr">I2C address for the matrix. Default address is 0x70, but it can be modified by soldering jumpers on the logic board</param>
        /// <param name="updateMode">Update mode, defaults to Automatic</param>
        public AdafruitController(int addr = 0x70, DisplayUpdateMode updateMode = DisplayUpdateMode.Automatic)
        {
            InitializeConnection(addr);
            StartDisplay();
            SetBlink(Blink.Off);
            SetBrightness(BRIGHTNESS_HI);
            UpdateMode = updateMode;
            CreateBuffers();
        }

        /// <summary>
        /// Initializes the connection
        /// </summary>
        /// <param name="addr">I2C address for the matrix.</param>
        private void InitializeConnection(int addr)
        {
            driver = new I2cDriver(Raspberry.IO.GeneralPurpose.ProcessorPin.Pin2, Raspberry.IO.GeneralPurpose.ProcessorPin.Pin3);
            connection = driver.Connect(addr);
        }

        /// <summary>
        /// Initializes the display
        /// </summary>
        private void StartDisplay()
        {
            connection.Write(START_DISPLAY);
        }

        /// <summary>
        /// Sets global blink mode
        /// </summary>
        /// <param name="blink">Choices are off, 1/2 Hz, 1 Hz or 2 Hz (2 Hz appears to do nothing currently)</param>
        public override void SetBlink(Blink blink)
        {
            var blinkCommand = BLINK_CMD | BLINK_DISPLAY_ON | (int)blink;
            connection.Write((byte)blinkCommand);
        }

        /// <summary>
        /// Sets global brightness
        /// </summary>
        /// <param name="brightness">Ranges from 0 (dull) to 15 (bright)</param>
        public override void SetBrightness(byte brightness)
        {
            brightness =
                brightness > BRIGHTNESS_HI ? BRIGHTNESS_HI :
                brightness < BRIGHTNESS_LO ? BRIGHTNESS_LO :
                brightness;

            var brightnessCommand = BRIGHTNESS_CMD | brightness;
            connection.Write((byte)brightnessCommand);
        }


        /// <summary>
        /// Disposes of the controller and underlying resources
        /// </summary>
        public override void Dispose()
        {
            driver.Dispose();
        }

        /// <summary>
        /// Plots the display buffer to the board, automatically called with Update Mode is set to Automatic
        /// </summary>
        public override void Update()
        {
            var data = new List<byte> { 0x0 };
            foreach (var item in displayBuffer)
            {
                var buffer = new byte[1];
                item.CopyTo(buffer, 0);
                data.Add(buffer[0]);
                // Must add zero padding, because the matrix expects uint16 for each segment
                data.Add(0x0);
            }
            connection.Write(data.ToArray());
        }
    }
}
