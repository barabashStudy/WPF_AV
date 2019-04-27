using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace WpfAV
{
    /// <summary>
    /// класс, создающий объект для сканирования
    /// (подписчик и источник сообщений(scanobj))
    /// </summary>
    class FilePreparer : IObserver<string>, IObservable<ScanObject1>  //наблюдатель (подписчик) //+отправитель объекта
    {
        private IDisposable cancellation;
        private List<IObserver<ScanObject1>> observers;
        private List<ScanObject1> objects;
        public FilePreparer()
        {
            observers = new List<IObserver<ScanObject1>>();
            objects = new List<ScanObject1>();
        }

        public void Subscribe(FileFinder provider)
        {
            cancellation = provider.Subscribe(this);
        }

        public void OnCompleted()
        {
            if (cancellation != null)
                cancellation.Dispose();
        }
        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(string path)
        {
            //string fileName = Path.GetFileName(path);
            //Mutex mutex = new Mutex(false, 'M' + fileName);
            //mutex.WaitOne();
            //var mmFile = MemoryMappedFile.CreateFromFile(path, FileMode.Open, fileName, 0);
            //var fileAccessor = mmFile.CreateViewAccessor();

            //uint offsetPE = fileAccessor.ReadUInt32(60);
            //if (offsetPE <= fileAccessor.Capacity)
            //{
            //    byte[] signature = new byte[4];
            //    fileAccessor.ReadArray(offsetPE, signature, 0, 4);
            //    if (Encoding.ASCII.GetString(signature) == "PE\0\0")
            //    {
            //        ushort magic = fileAccessor.ReadUInt16(offsetPE + 24);
            //        uint PEHeaderSize;
            //        if (magic == 267)
            //            PEHeaderSize = 248;
            //        else  //PE32+ = 523
            //            PEHeaderSize = 264;
            //        ushort numberOfSection = fileAccessor.ReadUInt16(offsetPE + 6);
            //        uint firstSection = offsetPE + PEHeaderSize;
            //        for (int i = 0; i < numberOfSection; i++)
            //        {
            //            long currentSection = firstSection + i * 40;
            //            uint characteristics = fileAccessor.ReadUInt32(currentSection + 36);
            //            uint flag = 0x20000000;
            //            if ((characteristics & flag) == flag)
            //            {
            //                uint lenght = fileAccessor.ReadUInt32(currentSection + 16);
            //                uint start = fileAccessor.ReadUInt32(currentSection + 20);
            //                var sectionAccessor = mmFile.CreateViewAccessor(start, lenght, MemoryMappedFileAccess.Read);
            //                ScanObject1 obj = new ScanObject1(sectionAccessor, mmFile, path);


            //                objects.Add(obj);
            //                foreach (var observer in observers)
            //                    observer.OnNext(obj);
            //            }
            //        }
            //    }
            //}
            //fileAccessor.Flush();
            //fileAccessor.Dispose();
            //mmFile.Dispose();
            //mutex.ReleaseMutex();

            //---------------------------------------------
            string fileName = Path.GetFileName(path);
            BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open));
            reader.BaseStream.Position = 0;


            byte[] text1;
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                text1 = ms.ToArray();
            }


            ScanObject1 obj = new ScanObject1(path, text1);
            objects.Add(obj);
            reader.Close();
            foreach (var observer in observers)
                observer.OnNext(obj);
        }

        public IDisposable Subscribe(IObserver<ScanObject1> observer)        //
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                foreach (ScanObject1 obj in objects)
                    observer.OnNext(obj);
            }
            return new PreparerUnsubscriber(observers, observer);
        }
    }

    class PreparerUnsubscriber : IDisposable
    {
        private List<IObserver<ScanObject1>> observers;
        private IObserver<ScanObject1> observer;

        public PreparerUnsubscriber(List<IObserver<ScanObject1>> observers, IObserver<ScanObject1> observer)
        {
            this.observers = observers;
            this.observer = observer;
        }
        public void Dispose()
        {
            if (observers.Contains(observer))
                observers.Remove(observer);
        }
    }
}
