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
        private int[] fileSizes;

        public FileSource(string[] filePaths)
        {
            this.FilePaths = filePaths;
            this.fileSizes = new int[this.FilePaths.Length];

            for (int i = 0; i < this.FilePaths.Length; i++)
            {
                FileInfo info = new FileInfo(this.FilePaths[i]);
                fileSizes[i] = (int)info.Length;
            }
        }

        public byte[] GetFiles()
        {
            int totalSize = this.fileSizes.Sum();

            byte[] result = new byte[totalSize];
            int byteIndex = 0;
            for (int i = 0; i < this.FilePaths.Length; i++)
            {
                using (BinaryReader reader = new BinaryReader(File.Open(this.FilePaths[i], FileMode.Open)))
                {
                    reader.Read(result, byteIndex, this.fileSizes[i]);
                    byteIndex += this.fileSizes[i];
                }
            }

            return result;
        }
    }
}
