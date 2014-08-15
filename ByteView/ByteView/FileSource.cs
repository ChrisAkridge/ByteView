using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteView
{
    public sealed class FileSource
    {
        public string[] FilePaths { get; private set; }
        public int[] FileSizes { get; private set; }

        public FileSource(string[] filePaths)
        {
            this.FilePaths = filePaths;
            this.FileSizes = new int[this.FilePaths.Length];

            for (int i = 0; i < this.FilePaths.Length; i++)
            {
                FileInfo info = new FileInfo(this.FilePaths[i]);
                FileSizes[i] = (int)info.Length;
            }
        }

        public byte[] GetFiles()
        {
            int totalSize = this.FileSizes.Sum();

            byte[] result = new byte[totalSize];
            int byteIndex = 0;
            for (int i = 0; i < this.FilePaths.Length; i++)
            {
                using (BinaryReader reader = new BinaryReader(File.Open(this.FilePaths[i], FileMode.Open)))
                {
                    reader.Read(result, byteIndex, this.FileSizes[i]);
                    byteIndex += this.FileSizes[i];
                }
            }

            return result;
        }
    }
}
