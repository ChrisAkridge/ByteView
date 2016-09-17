using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteView
{
	/// <summary>
	/// A sequence of file paths.
	/// </summary>
    public sealed class FileSource
    {
		/// <summary>
		/// An array of all file paths in this source.
		/// </summary>
		private string[] filePaths;

		/// <summary>
		/// An array of the sizes of all files in this source.
		/// </summary>
		private int[] fileSizes;

		/// <summary>
		/// Gets an immutable list of all file paths in this source.
		/// </summary>
		public IReadOnlyList<string> FilePaths
		{
			get
			{
				return Array.AsReadOnly(filePaths);
			}
		}
		
		/// <summary>
		/// Gets an immutable list of the sizes of all file paths in this source.
		/// </summary>
		public IReadOnlyList<int> FileSizes
		{
			get
			{
				return Array.AsReadOnly(fileSizes);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FileSource"/> class.
		/// </summary>
		/// <param name="cFilePaths">An array of file paths.</param>
        public FileSource(string[] cFilePaths)
        {
			if (cFilePaths == null || cFilePaths.Length == 0) throw new ArgumentException("The provided file paths array was null or empty.", nameof(cFilePaths));

			filePaths = cFilePaths;
			fileSizes = new int[filePaths.Length];

            for (int i = 0; i < filePaths.Length; i++)
            {
                FileInfo info = new FileInfo(filePaths[i]);
                fileSizes[i] = (int)info.Length;
            }
        }

		/// <summary>
		/// Returns the bytes of every file in this source, in order, as a byte array.
		/// </summary>
		/// <returns>The bytes of every file.</returns>
        public byte[] GetFiles()
        {
            int totalSize = fileSizes.Sum();

            byte[] result = new byte[totalSize];
            int byteIndex = 0;
            for (int i = 0; i < filePaths.Length; i++)
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filePaths[i], FileMode.Open)))
                {
                    reader.Read(result, byteIndex, fileSizes[i]);
                    byteIndex += fileSizes[i];
                }
            }

            return result;
        }
    }
}
