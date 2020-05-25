using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ByteView
{
    internal static class Helpers
    {
        /// <summary>
        /// Checks if an integer's square root is an integer.
        /// </summary>
        /// <param name="n">The integer to check.</param>
        /// <returns>
        /// True if <paramref name="n" /> is a perfect square, false if it is not.
        /// </returns>
        public static bool IsPerfectSquare(int n)
        {
            if (n < 1) { return false; }

            int squareRoot = (int)Math.Sqrt(n);
            return squareRoot * squareRoot == n;
        }

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

        public static int GetTextHeight(int imageHeight) => Math.Max(24, imageHeight / 30);

        public static bool TryShortenFilePath(string filePath, out string result)
        {
            char pathSeparator = Path.DirectorySeparatorChar;
            List<string> parts = filePath.Split(pathSeparator).ToList();

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

        internal static string GetLFPImageText(IList<string> list) =>
            list.Count == 1 ? list.First() : $"{list.First()} + {list.Count - 1}";
    }
}
