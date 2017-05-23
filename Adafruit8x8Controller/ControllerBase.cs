using System;
using System.Collections;

namespace Adafruit8x8Controller
{
    public abstract class ControllerBase : IDisposable
    {
        protected BitArray[] displayBuffer = new BitArray[8];

        /// <summary>
        /// Sets update mode for the controller. Automatic means that any draw command will be immediately issued to the matrix, while manual requires an explicit call to Update()
        /// </summary>
        public DisplayUpdateMode UpdateMode { get; set; }

        /// <summary>
        /// Creates display column/row buffers
        /// </summary>
        protected void CreateBuffers()
        {
            for (var i = 0; i < displayBuffer.Length; i++)
                displayBuffer[i] = new BitArray(8, false);
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

        public abstract void SetBlink(Blink blink);
        public abstract void SetBrightness(byte brightness);
        public abstract void Update();
        public abstract void Dispose();
    }
}