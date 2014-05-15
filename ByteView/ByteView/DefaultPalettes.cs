using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteView
{
    public static class DefaultPalettes
    {
        public static int[] OneBppGrayscale { get; private set; }
        public static int[] TwoBppGrayscale { get; private set; }
        public static int[] FourBppGrayscale { get; private set; }
        public static int[] FourBppRGB121 { get; private set; }
        public static int[] EightBppGrayscale { get; private set; }
        public static int[] EightBppRGB332 { get; private set; }
        public static int[] EightBppARGB2222 { get; private set; }
        public static int[] SixteenBppRGB565 { get; private set; }
        public static int[] SixteenBppARGB4444 { get; private set; }

        static DefaultPalettes()
        {
            OneBppGrayscale = new int[2];
            TwoBppGrayscale = new int[4];
            FourBppGrayscale = new int[16];
            FourBppRGB121 = new int[16];
            EightBppGrayscale = new int[256];
            EightBppRGB332 = new int[256];
            EightBppARGB2222 = new int[256];
            SixteenBppRGB565 = new int[65536];
            SixteenBppARGB4444 = new int[65536];

            CreatePalettes();
        }

        private static void CreatePalettes()
        {
            byte[] twoBitRange = GenerateRange(4);
            byte[] threeBitRange = GenerateRange(8);
            byte[] fourBitRange = GenerateRange(16);
            byte[] fiveBitRange = GenerateRange(32);
            byte[] sixBitRange = GenerateRange(64);

            int i;

            // 1bpp grayscale
            OneBppGrayscale[0] = unchecked((int)0xFF000000);
            OneBppGrayscale[1] = unchecked((int)0xFFFFFFFF);

            // 2bpp grayscale
            for (i = 0; i < 4; i++)
            {
                TwoBppGrayscale[i] = (0xFF << 24) + (twoBitRange[i] << 16) + (twoBitRange[i] << 8) + (twoBitRange[i]);
            }

            // 4bpp grayscale
            for (i = 0; i < 16; i++)
            {
                FourBppGrayscale[i] = (0xFF << 24) + (fourBitRange[i] << 16) + (fourBitRange[i] << 8) + fourBitRange[i];
            }

            // 4bpp RGB
            for (i = 0; i < 16; i++)
            {
                int red = 0xFF * ((i & 0x08) >> 3); // 0x08 == 1000_2
                byte green = twoBitRange[(i & 0x06) >> 1]; // 0x06 == 0110_2
                int blue = 0xFF * (i & 0x01);
                FourBppRGB121[i] = (0xFF << 24) + (red << 16) + (green << 8) + blue;
            }

            // 8bpp grayscale
            for (i = 0; i < 256; i++)
            {
                EightBppGrayscale[i] = (0xFF << 24) + (i << 16) + (i << 8) + i;
            }

            // 8bpp RGB
            for (i = 0; i < 256; i++)
            {
                byte red = threeBitRange[(i & 0xE0) >> 5]; // 0xE0 == 11100000_2
                byte green = threeBitRange[(i & 0x1C) >> 2]; // 0x1C = 00011100_2
                byte blue = threeBitRange[i & 0x03]; // 0x03 == 00000011_2
                EightBppRGB332[i] = (0xFF << 24) + (red << 16) + (green << 8) + blue;
            }

            // 8bpp ARGB
            for (i = 0; i < 256; i++)
            {
                byte alpha = twoBitRange[(i & 0xC0) >> 6]; // 0xC0 == 11000000_2
                byte red = twoBitRange[(i & 0x30) >> 4]; // 0x30 == 00110000_2
                byte green = twoBitRange[(i & 0x0C) >> 2]; // 0x0C == 00001100_2
                byte blue = twoBitRange[i & 0x03]; // 0x03 == 00000011_2
                EightBppARGB2222[i] = (alpha << 24) + (red << 16) + (green << 8) + blue;
            }

            // 16bpp RGB
            for (i = 0; i < 65536; i++)
            {
                byte red = fiveBitRange[(i & 0xF800) >> 11]; // 0xF800 == 11111000 00000000_2
                byte green = sixBitRange[(i & 0x07E0) >> 5]; // 0x07E0 == 00000111 11100000_2
                byte blue = fiveBitRange[i & 0x001F]; // 0x001F == 00000000 00011111_2
                SixteenBppRGB565[i] = (0xFF << 24) + (red << 16) + (green << 8) + blue;
            }

            // 16bpp ARGB
            for (i = 0; i < 65536; i++)
            {
                byte alpha = fourBitRange[(i & 0xF000) >> 12]; // 0xF000 == 11110000 00000000_2
                byte red = fourBitRange[(i & 0x0F00) >> 8]; // 0x0F000 == 00001111 00000000_2
                byte green = fourBitRange[(i & 0x00F0) >> 4]; // 0x00F0 == 00000000 11110000_2
                byte blue = fourBitRange[i & 0x000F]; // 0x000F == 00000000 00001111_2
                SixteenBppARGB4444[i] = (alpha << 24) + (red << 16) + (green << 8) + blue;
            }
        }

        private static byte[] GenerateRange(int divisor)
        {
            byte[] result = new byte[divisor];
            for (int i = 0; i < divisor; i++)
            {
                result[i] = (byte)(255f * (i / (float)(divisor - 1)));
            }
            return result;
        }

        public static int[] GetPalette(BitDepth depth, ColorMode mode)
        {
            if (mode == ColorMode.Grayscale)
            {
                if (depth == BitDepth.OneBpp)
                {
                    return OneBppGrayscale;
                }
                else if (depth == BitDepth.TwoBpp)
                {
                    return TwoBppGrayscale;
                }
                else if (depth == BitDepth.FourBpp)
                {
                    return FourBppGrayscale;
                }
                else if (depth == BitDepth.EightBpp)
                {
                    return EightBppGrayscale;
                }
            }
            else if (mode == ColorMode.RGB)
            {
                if (depth == BitDepth.FourBpp)
                {
                    return FourBppRGB121;
                }
                else if (depth == BitDepth.EightBpp)
                {
                    return EightBppRGB332;
                }
                else if (depth == BitDepth.SixteenBpp)
                {
                    return SixteenBppRGB565;
                }
            }
            else if (mode == ColorMode.ARGB)
            {
                if (depth == BitDepth.EightBpp)
                {
                    return EightBppARGB2222;
                }
                else if (depth == BitDepth.SixteenBpp)
                {
                    return SixteenBppARGB4444;
                }
            }
            else
            {
                throw new ArgumentException("No default palette for this color mode.");
            }

            throw new Exception("You should never see this message.");
        }
    }
}
