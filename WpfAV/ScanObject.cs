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
    class ScanObject
    {
        protected MemoryMappedViewAccessor accessor;
        protected MemoryMappedFile mmFile;
        protected string path;
        public ScanObject(MemoryMappedViewAccessor accessor, MemoryMappedFile mmFile, string path)
        {
            this.accessor = accessor;
            this.mmFile = mmFile;
            this.path = path;
        }
        public byte[] GetBytes(int offset, int count)
        {
            byte[] array = new byte[count];
            accessor.ReadArray(offset, array, 0, count);
            return array;
        }
        public long Lenght
        {
            get { return accessor.Capacity; }
        }
        public string Path
        {
            get { return path; }
        }
        public void Close()
        {
            accessor.Flush();
        }
    }
}
