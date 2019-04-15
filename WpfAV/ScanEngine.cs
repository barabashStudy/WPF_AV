using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace WpfAV
{
    /// <summary>
    ///  класс для сканирования объектов ScanObject
    ///  (подписчик и отправитель(вирусИнфо)
    /// </summary>
    class ScanEngine : IObserver<ScanObject>, IObservable<VirusInfo>    //подписчик объекта //+отправитель инфы о вирусе
    {
        private List<RecordsDictionary> AVBases;
        private List<IObserver<VirusInfo>> observers;
        private List<VirusInfo> Viruses;
        public int CountBases
        {
            get { return AVBases.Count; }
        }
        public ScanEngine()
        {
            AVBases = new List<RecordsDictionary>();
            observers = new List<IObserver<VirusInfo>>();
            Viruses = new List<VirusInfo>();
        }

        public void AddBase(RecordsDictionary avBase)
        {
            AVBases.Add(avBase);
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(ScanObject value)
        {
            Scan(value);
        }

        public void Scan(ScanObject scanObject)
        {
            if (scanObject != null)
            {
                byte[] firstBytes = new byte[4];
                long lenght = scanObject.Lenght;
                int offset = 0;
                do
                {
                    firstBytes = scanObject.GetBytes(offset, 4);
                    foreach (RecordsDictionary avBase in AVBases)
                    {
                        if (avBase.ContainsKey(firstBytes))
                        {
                            int countRecords = avBase.CountRecords(firstBytes);
                            for (int i = 0; i < countRecords; i++)
                            {
                                AVRecord record = avBase.GetRecord(firstBytes, i);
                                //if ((offset >= record.OffsetStart) && (offset <= record.OffsetEnd))
                                {
                                    byte[] hash = GetHash(scanObject.GetBytes(offset, (int)record.Lenght));
                                    if (IsEqualArray(hash, record.Hash))
                                    {
                                        VirusInfo virus = new VirusInfo(scanObject.Path, record.Name);
                                        Viruses.Add(virus);

                                        vir.virInfo = virus;

                                        foreach (var observer in observers)
                                            observer.OnNext(virus);

                                    }
                                }
                            }
                        }
                    }
                    offset++;
                } while (offset <= lenght - 5);
            }
            scanObject.Close();
        }

        public byte[] GetHash(byte[] text)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] hash = sha1.ComputeHash(text);
            return hash;
        }

        public bool IsEqualArray(byte[] hash1, byte[] hash2)
        {
            int lenght = hash1.Length;
            if (lenght != hash2.Length)
                return false;
            for (int i = 0; i < lenght; i++)
            {
                if (hash1[i] != hash2[i])
                    return false;
            }
            return true;
        }

        public IDisposable Subscribe(IObserver<VirusInfo> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
                foreach (VirusInfo virus in Viruses)
                    observer.OnNext(virus);
            }
            return new ScanEngineUnsubscriber(observers, observer);
        }
    }

    class ScanEngineUnsubscriber : IDisposable
    {
        private List<IObserver<VirusInfo>> observers;
        private IObserver<VirusInfo> observer;

        public ScanEngineUnsubscriber(List<IObserver<VirusInfo>> observers,
                                      IObserver<VirusInfo> observer)
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
