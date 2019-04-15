using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAV
{
    /// <summary>
    /// класс, реализующий паттерн «Наблюдатель»
    /// (поставщик сообщений)
    /// </summary>
    class FileObservable : IObservable<string>      //поставщик сообщений
    {
        protected List<IObserver<string>> observers;
        protected List<string> files;

        public FileObservable()
        {
            observers = new List<IObserver<string>>();
            files = new List<string>();
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                foreach (string file in files)
                    observer.OnNext(file);
            }
            return new Unsubscriber(observers, observer);
        }

        public void AddFile(string path)
        {
            if (!files.Contains(path))
            {
                files.Add(path);
            }
        }

        public void NotifyObservers(string path)
        {
            foreach (IObserver<string> observer in observers)
                observer.OnNext(path);
        }
    }


    class Unsubscriber : IDisposable
    {
        private List<IObserver<string>> observers;
        private IObserver<string> observer;

        public Unsubscriber(List<IObserver<string>> observers, IObserver<string> observer)
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
