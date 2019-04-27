using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.MemoryMappedFiles;


namespace WpfAV
{
    /// <summary>
    /// класс объекта для сканирования
    /// (ничего)
    /// </summary>
    class ScanObject1
    {
        protected MemoryMappedViewAccessor accessor;
        protected MemoryMappedFile mmFile;
        protected string path;
        protected byte[] arrayByte;

        public ScanObject1(string path, byte[] arrayByte)
        {
            //this.accessor = accessor;
            //this.mmFile = mmFile;
            this.path = path;
            this.arrayByte = arrayByte;

        }
        public byte[] GetBytes(int offset, int count)
        {
            byte[] array = new byte[count];
            Array.ConstrainedCopy(arrayByte, offset, array, 0, count);    
            //accessor.ReadArray(offset, array, 0, count);
            return array;
        }
        public long Lenght
        {
            get { return arrayByte.LongLength; }
        }
        public string Path
        {
            get { return path; }
        }
        public void Close()
        {
            //ScanObject1
            accessor.Flush();
        }
    }
}
