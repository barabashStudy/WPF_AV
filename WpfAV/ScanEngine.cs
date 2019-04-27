using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace WpfAV
{
    /// <summary>
    ///  класс для сканирования объектов scanObject1
    ///  (подписчик и отправитель(вирусИнфо)
    /// </summary>
    class ScanEngine : IObserver<ScanObject1>, IObservable<VirusInfo>    //подписчик объекта //+отправитель инфы о вирусе
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

        public void OnNext(ScanObject1 value)
        {
            Scan(value);
        }

        public void Scan(ScanObject1 scanObject1)
        {

            //BinaryReader reader = new BinaryReader(File.Open(@"C:\Users\saske\Desktop\scanTest\Viber.exe", FileMode.Open));
            //reader.BaseStream.Position = 0;


            //byte[] text1;
            //byte[] buffer = new byte[16 * 1024];
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    int read;
            //    while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
            //    {
            //        ms.Write(buffer, 0, read);
            //    }
            //    text1 = ms.ToArray();
            //}

            int s = 0;


            if (scanObject1 != null)
            {
                byte[] firstBytes = new byte[4];
                long lenght = scanObject1.Lenght;
                //long lenght = text1.Length;

                int offset = 0;
                do
                {
                    //firstBytes[0] = text1[0];
                    //firstBytes[1] = text1[1];
                    //firstBytes[2] = text1[2];
                    //firstBytes[3] = text1[3];

                    firstBytes = scanObject1.GetBytes(offset, 4);
                    foreach (RecordsDictionary avBase in AVBases)
                    {
                        if (avBase.ContainsKey(firstBytes))
                        {
                            int countRecords = avBase.CountRecords(firstBytes);
                            for (int i = 0; i < countRecords; i++)
                            {
                                AVRecord record = avBase.GetRecord(firstBytes, i);
                                if ((offset >= record.OffsetStart) && (offset <= record.OffsetEnd))
                                {

                                    //int offset;
                                    int count = Convert.ToInt32(record.Lenght);

                                    //byte[] array = new byte[count];
                                    //reader.BaseStream.Position = 0;
                                    //array = reader.ReadBytes(count);
                                    //byte[] hash = GetHash(array);
                                    byte[] hash = GetHash(scanObject1.GetBytes(offset, (int)record.Lenght));

                                    if (IsEqualArray(hash, record.Hash))
                                    {
                                        VirusInfo virus = new VirusInfo(scanObject1.Path, record.Name);

                                        if (!Viruses.Contains(virus))
                                            Viruses.Add(virus);

                                        s = 1;

                                        foreach (var observer in observers)
                                            observer.OnNext(virus);



                                        //break;

                                    }
                                }
                            }
                        }
                    }
                    offset++;
                } while (offset <= lenght - 5 & s == 0);

                //observers[0].OnCompleted();

                //if (s == 1)
                //{
                //    VirusInfo end = new VirusInfo("completed", "scan");
                //    foreach (var observer in observers)
                //        observer.OnNext(end);
                //}

            }
            //scanObject1.Close();
            //reader.Close();
        }


        public void ModifyText(System.Windows.Controls.ListBox myLB)
        {
            foreach (var virus in Viruses)
                myLB.Items.Add(virus.VirusName + "   " + virus.Path);
        }





        public void disp()
        {
            //AVBases.Clear();
            observers.Clear();
            Viruses.Clear();

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

        public ScanEngineUnsubscriber(List<IObserver<VirusInfo>> observers, IObserver<VirusInfo> observer)
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
