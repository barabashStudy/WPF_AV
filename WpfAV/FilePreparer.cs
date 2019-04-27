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
