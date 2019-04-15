using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAV
{
    [Serializable]
    class AVRecord
    {
        private string name;
        private uint offsetStart;
        private uint offsetEnd;
        private uint lenght;
        private byte[] firstBytes;
        private byte[] hash;
        private string fileType;
        public AVRecord(string name, uint offsetStart, uint offsetEnd, uint lenght, byte[] firstBytes, byte[] hash, string fileType)
        {
            this.name = name;
            this.offsetStart = offsetStart;
            this.offsetEnd = offsetEnd;
            this.lenght = lenght;
            this.firstBytes = firstBytes;
            this.hash = hash;
            this.fileType = fileType;
        }
        public byte[] FirstBytes
        {
            get
            { return firstBytes; }
        }

        public byte[] Hash
        {
            get { return hash; }
        }

        public uint OffsetEnd
        {
            get { return offsetEnd; }
        }

        public uint Lenght
        {
            get { return lenght; }
        }

        public uint OffsetStart
        {
            get { return offsetStart; }
        }

        public string Name
        {
            get { return name; }
        }

    }
}
