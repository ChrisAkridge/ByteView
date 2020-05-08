using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteView
{
    internal static class Helpers
    {
        /// <summary>
        /// Generates a suffix for file sizes.
        /// </summary>
        /// <param name="fileSize"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GenerateFileSizeAbbreviation(ulong fileSize, out int number)
        {
            // TODO: This method is a GREAT candidate to go into ChrisAkridge.Common.
            char[] prefixes = { 'K', 'M', 'G', 'T', 'P', 'E', 'Z', 'Y' };

            if (fileSize < 1024UL)
            {
                number = (int)fileSize;
                return "B";
            }

            int prefixNumber = -1;
            while (fileSize >= 1024UL)
            {
                fileSize /= 1024UL;
                prefixNumber++;
            }

            number = (int)fileSize;
            return string.Concat(prefixes[prefixNumber], "B");
        }

        public static int GetTextHeight(int imageHeight)
        {
            return Math.Max(24, imageHeight / 30);
        }

        public static bool TryShortenFilePath(string filePath, out string result)
        {
            var pathSeparator = Path.DirectorySeparatorChar;
            var parts = filePath.Split(pathSeparator).ToList();

            string driveLetter = parts[0];
            parts.RemoveAt(0);

            string fileName = parts[parts.Count - 1];
            parts.RemoveAt(parts.Count - 1);

            string longestPart = parts.OrderByDescending(p => p.Length).First();

            if (longestPart.Length == 3)
            {
                result = filePath;
                return false;
            }

            int longestPartIndex = parts.IndexOf(longestPart);
            parts[longestPartIndex] = "...";

            result = string.Concat(driveLetter, pathSeparator, string.Join(pathSeparator.ToString(), parts), pathSeparator, fileName);
            return true;
        }

        public static decimal GetBytesPerPixel(BitDepth depth)
        {
            switch (depth)
            {
                case BitDepth.OneBpp: return 1m / 8m;
                case BitDepth.TwoBpp: return 1m / 4m;
                case BitDepth.FourBpp: return 1m / 2m;
                case BitDepth.EightBpp: return 1m;
                case BitDepth.SixteenBpp: return 2m;
                case BitDepth.TwentyFourBpp: return 3m;
                case BitDepth.ThirtyTwoBpp: return 4m;
                case BitDepth.Invalid:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal static string GetLFPImageText(List<string> list)
        {
            if (list.Count == 1)
            {
                return list.First();
            }

            return $"{list.First()} + {list.Count - 1}";
        }
    }
}
