using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Threading;


namespace WpfAV
{
    /// <summary>
    /// класс, создающий объект для сканирования
    /// (подписчик и источник сообщений(scanobj))
    /// </summary>
    class FilePreparer : IObserver<string>, IObservable<ScanObject>  //наблюдатель (подписчик) //+отправитель объекта
    {
        private IDisposable cancellation;
        private List<IObserver<ScanObject>> observers;
        private List<ScanObject> objects;
        public FilePreparer()
        {
            observers = new List<IObserver<ScanObject>>();
            objects = new List<ScanObject>();
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
            //MessageBox.Show(path);
            string fileName = Path.GetFileName(path);
            Mutex mutex = new Mutex(false, 'M' + fileName);
            mutex.WaitOne();
            var mmFile = MemoryMappedFile.CreateFromFile(path, FileMode.Open, fileName, 0);
            var fileAccessor = mmFile.CreateViewAccessor();

            uint offsetPE = fileAccessor.ReadUInt32(60);
            if (offsetPE <= fileAccessor.Capacity)
            {
                byte[] signature = new byte[4];
                fileAccessor.ReadArray(offsetPE, signature, 0, 4);
                if (Encoding.ASCII.GetString(signature) == "PE\0\0")
                {
                    ushort magic = fileAccessor.ReadUInt16(offsetPE + 24);
                    uint PEHeaderSize;
                    if (magic == 267)
                        PEHeaderSize = 248;
                    else  //PE32+ = 523
                        PEHeaderSize = 264;
                    ushort numberOfSection = fileAccessor.ReadUInt16(offsetPE + 6);
                    uint firstSection = offsetPE + PEHeaderSize;
                    for (int i = 0; i < numberOfSection; i++)
                    {
                        long currentSection = firstSection + i * 40;
                        uint characteristics = fileAccessor.ReadUInt32(currentSection + 36);
                        uint flag = 0x20000000;
                        if ((characteristics & flag) == flag)
                        {
                            uint lenght = fileAccessor.ReadUInt32(currentSection + 16);
                            uint start = fileAccessor.ReadUInt32(currentSection + 20);
                            var sectionAccessor = mmFile.CreateViewAccessor(start, lenght, MemoryMappedFileAccess.Read);
                            ScanObject obj = new ScanObject(sectionAccessor, mmFile, path);
                            objects.Add(obj);
                            foreach (var observer in observers)
                                observer.OnNext(obj);
                        }
                    }
                }
            }
            fileAccessor.Flush();
            fileAccessor.Dispose();
            mmFile.Dispose();
            mutex.ReleaseMutex();
        }

        public IDisposable Subscribe(IObserver<ScanObject> observer)        //
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                foreach (ScanObject obj in objects)
                    observer.OnNext(obj);
            }
            return new PreparerUnsubscriber(observers, observer);
        }
    }

    class PreparerUnsubscriber : IDisposable
    {
        private List<IObserver<ScanObject>> observers;
        private IObserver<ScanObject> observer;

        public PreparerUnsubscriber(List<IObserver<ScanObject>> observers, IObserver<ScanObject> observer)
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
