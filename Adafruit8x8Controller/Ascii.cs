﻿using System.Collections.Generic;

namespace Adafruit8x8Controller
{
    public static class Ascii
    {
        public static int[] GetChar(char character)
        {
            return chars[character];
        }
        static Dictionary<char, int[]> chars = new Dictionary<char, int[]>
        {
            { 'A', new int[] { 0x06, 0x09, 0x09, 0x0F, 0x09, 0x09, 0x00, 0x00 } },
            { 'B', new int[] { 0x07, 0x09, 0x07, 0x09, 0x09, 0x07, 0x00, 0x00 } },
            { 'C', new int[] { 0x0E, 0x01, 0x01, 0x01, 0x01, 0x0E, 0x00, 0x00 } },
            { 'D', new int[] { 0x07, 0x09, 0x09, 0x09, 0x09, 0x07, 0x00, 0x00 } },
            { 'E', new int[] { 0x0F, 0x01, 0x07, 0x01, 0x01, 0x0F, 0x00, 0x00 } },
            { 'F', new int[] { 0x0F, 0x01, 0x07, 0x01, 0x01, 0x01, 0x00, 0x00 } },
            { 'G', new int[] { 0x06, 0x09, 0x01, 0x0D, 0x09, 0x06, 0x00, 0x00 } },
            { 'H', new int[] { 0x09, 0x09, 0x0F, 0x09, 0x09, 0x09, 0x00, 0x00 } },
            { 'I', new int[] { 0x0E, 0x04, 0x04, 0x04, 0x04, 0x0E, 0x00, 0x00 } },
            { 'J', new int[] { 0x0F, 0x08, 0x08, 0x08, 0x09, 0x06, 0x00, 0x00 } },
            { 'K', new int[] { 0x09, 0x05, 0x03, 0x03, 0x05, 0x09, 0x00, 0x00 } },
            { 'L', new int[] { 0x01, 0x01, 0x01, 0x01, 0x01, 0x0F, 0x00, 0x00 } },
            { 'M', new int[] { 0x11, 0x1B, 0x15, 0x11, 0x11, 0x11, 0x00, 0x00 } },
            { 'N', new int[] { 0x11, 0x13, 0x15, 0x15, 0x19, 0x11, 0x00, 0x00 } },
            { 'O', new int[] { 0x06, 0x09, 0x09, 0x09, 0x09, 0x06, 0x00, 0x00 } },
            { 'P', new int[] { 0x07, 0x09, 0x09, 0x07, 0x01, 0x01, 0x00, 0x00 } },
            { 'Q', new int[] { 0x06, 0x09, 0x09, 0x09, 0x0D, 0x0E, 0x10, 0x00 } },
            { 'R', new int[] { 0x07, 0x09, 0x09, 0x07, 0x05, 0x09, 0x00, 0x00 } },
            { 'S', new int[] { 0x06, 0x09, 0x02, 0x04, 0x09, 0x06, 0x00, 0x00 } },
            { 'T', new int[] { 0x1F, 0x04, 0x04, 0x04, 0x04, 0x04, 0x00, 0x00 } },
            { 'U', new int[] { 0x09, 0x09, 0x09, 0x09, 0x09, 0x06, 0x00, 0x00 } },
            { 'V', new int[] { 0x11, 0x11, 0x11, 0x11, 0x0A, 0x04, 0x00, 0x00 } },
            { 'W', new int[] { 0x11, 0x11, 0x11, 0x11, 0x15, 0x0A, 0x00, 0x00 } },
            { 'X', new int[] { 0x11, 0x0A, 0x04, 0x0A, 0x11, 0x11, 0x00, 0x00 } },
            { 'Y', new int[] { 0x11, 0x0A, 0x04, 0x04, 0x04, 0x04, 0x00, 0x00 } },
            { 'Z', new int[] { 0x0F, 0x08, 0x04, 0x02, 0x01, 0x0F, 0x00, 0x00 } },
            { 'Å', new int[] { 0x06, 0x00, 0x06, 0x09, 0x0F, 0x09, 0x00, 0x00 } },
            { 'Ä', new int[] { 0x09, 0x00, 0x06, 0x09, 0x0F, 0x09, 0x00, 0x00 } },
            { 'Ö', new int[] { 0x09, 0x00, 0x06, 0x09, 0x09, 0x06, 0x00, 0x00 } },
            { '+', new int[] { 0x00, 0x02, 0x07, 0x02, 0x00, 0x00, 0x00, 0x00 } },
            { '-', new int[] { 0x00, 0x00, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00 } },
        };
    }
}
