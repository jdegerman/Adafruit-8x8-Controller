using Raspberry.IO.InterIntegratedCircuit;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Adafruit8x8Controller
{
    public enum DisplayUpdateMode
    {
        Automatic,
        Manual
    }
    public enum Blink
    {
        Off = 0x0,
        TwoHz = 0x1,
        OneHz = 0x2,
        HalfHz = 0x3
    }

    public class Controller : IDisposable
    {
        const int START_DISPLAY = 0x21;
        const int BLINK_CMD = 0x80;
        const int BLINK_DISPLAY_ON = 0x01;
        const int BRIGHTNESS_CMD = 0xE0;
        const byte BRIGHTNESS_LO = 0x0;
        const byte BRIGHTNESS_HI = 0xF;

        I2cDriver driver;
        I2cDeviceConnection connection;
        BitArray[] displayBuffer = new BitArray[8];

        /// <summary>
        /// Sets update mode for the controller. Automatic means that any draw command will be immediately issued to the matrix, while manual requires an explicit call to Update()
        /// </summary>
        public DisplayUpdateMode UpdateMode { get; set; }

        /// <summary>
        /// Constructor for the controller
        /// </summary>
        /// <param name="addr">I2C address for the matrix. Default address is 0x70, but it can be modified by soldering jumpers on the logic board</param>
        /// <param name="updateMode">Update mode, defaults to Automatic</param>
        public Controller(int addr = 0x70, DisplayUpdateMode updateMode = DisplayUpdateMode.Automatic)
        {
            InitializeConnection(addr);
            StartDisplay();
            SetBlink(Blink.Off);
            SetBrightness(BRIGHTNESS_HI);
            UpdateMode = updateMode;
            CreateBuffers();
        }

        /// <summary>
        /// Creates display column/row buffers
        /// </summary>
        private void CreateBuffers()
        {
            for (var i = 0; i < displayBuffer.Length; i++)
                displayBuffer[i] = new BitArray(8, false);
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
        public void StartDisplay()
        {
            connection.Write(START_DISPLAY);
        }

        /// <summary>
        /// Sets global blink mode
        /// </summary>
        /// <param name="blink">Choices are off, 1/2 Hz, 1 Hz or 2 Hz (2 Hz appears to do nothing currently)</param>
        public void SetBlink(Blink blink)
        {
            var blinkCommand = BLINK_CMD | BLINK_DISPLAY_ON | (int)blink;
            connection.Write((byte)blinkCommand);
        }

        /// <summary>
        /// Sets global brightness
        /// </summary>
        /// <param name="brightness">Ranges from 0 (dull) to 15 (bright)</param>
        public void SetBrightness(byte brightness)
        {
            brightness =
                brightness > BRIGHTNESS_HI ? BRIGHTNESS_HI :
                brightness < BRIGHTNESS_LO ? BRIGHTNESS_LO :
                brightness;

            var brightnessCommand = BRIGHTNESS_CMD | brightness;
            connection.Write((byte)brightnessCommand);
        }

        /// <summary>
        /// Clear the display (all pixels off)
        /// </summary>
        public void Clear()
        {
            for (var i = 0; i < displayBuffer.Length; i++)
                displayBuffer[i].SetAll(false);
            if (UpdateMode == DisplayUpdateMode.Automatic)
                Update();
        }

        /// <summary>
        /// Clears the display (all pixels on)
        /// </summary>
        public void Fill()
        {
            for (var i = 0; i < displayBuffer.Length; i++)
                displayBuffer[i].SetAll(true);
            if (UpdateMode == DisplayUpdateMode.Automatic)
                Update();
        }

        /// <summary>
        /// Sets the pixel at the specified coordinates to the specified state. 0x0 is currently the pixel on the top-right when the matrix is oriented with the connectors upwards.
        /// </summary>
        /// <param name="x">X-coordinate (wrapped at 8, no overflow allowed)</param>
        /// <param name="y">Y-coordinate (wrapped at 8, no overflow allowed)</param>
        /// <param name="state">Boolean state of specified pixel</param>
        public void SetPixel(int x, int y, bool state)
        {
            WrapCoords(ref x, ref y);
            displayBuffer[y].Set(x, state);
            if (UpdateMode == DisplayUpdateMode.Automatic)
                Update();
        }

        /// <summary>
        /// Gets the state of the pixel at the specified coordinates. 0x0 is currently the pixel on the top-right when the matrix is oriented with the connectors upwards.
        /// </summary>
        /// <param name="x">X-coordinate (wrapped at 8, no overflow allowed)</param>
        /// <param name="y">Y-coordinate (wrapped at 8, no overflow allowed)</param>
        /// <returns></returns>
        public bool GetPixel(int x, int y)
        {
            WrapCoords(ref x, ref y);
            return displayBuffer[y].Get(x);
        }

        /// <summary>
        /// Gets the state of the pixel at the specified coordinates. Note that the state depends on what the display buffer contains, not what the pixel's ACTUAL state is.
        /// 0x0 is currently the pixel on the top-right when the matrix is oriented with the connectors upwards.
        /// </summary>
        /// <param name="x">X-coordinate (wrapped at 8, no overflow allowed)</param>
        /// <param name="y">Y-coordinate (wrapped at 8, no overflow allowed)</param>
        public void TogglePixel(int x, int y)
        {
            WrapCoords(ref x, ref y);
            displayBuffer[y].Set(x, !displayBuffer[y].Get(x));
            if (UpdateMode == DisplayUpdateMode.Automatic)
                Update();
        }

        /// <summary>
        /// Disposes of the controller and underlying resources
        /// </summary>
        public void Dispose()
        {
            driver.Dispose();
        }

        /// <summary>
        /// Plots the display buffer to the board, automatically called with Update Mode is set to Automatic
        /// </summary>
        public void Update()
        {
            var data = new List<byte> { 0x0 };
            foreach(var item in displayBuffer)
            {
                var buffer = new byte[1];
                item.CopyTo(buffer, 0);
                data.Add(buffer[0]);
                // Must add zero padding, because the matrix expects uint16 for each segment
                data.Add(0x0);
            }
            connection.Write(data.ToArray());
        }

        void WrapCoords(ref int x, ref int y)
        {
            WrapX(ref x);
            WrapY(ref y);
        }

        void WrapY(ref int y)
        {
            y %= 8;
        }

        void WrapX(ref int x)
        {
            // Curious fact: The first pixel on each row is governed by bit 2^7, and the second pixel is governed by bit 2^0.
            // This method fixes the wrapping issue.
            x += 7;
            x %= 8;
        }
    }
}
