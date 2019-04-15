using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAV
{
    class RecordsDictionary
    {
        private SortedDictionary<byte[], List<AVRecord>> avBase;
        private int countKey;

        public RecordsDictionary()
        {
            avBase = new SortedDictionary<byte[], List<AVRecord>>(new ByteComparer());
            countKey = 0;
        }

        public void Add(AVRecord record)
        {
            if (!avBase.ContainsKey(record.FirstBytes))
            {
                List<AVRecord> records = new List<AVRecord>();
                records.Add(record);
                avBase.Add(record.FirstBytes, records);
                countKey++;
            }
            else
            {
                avBase[record.FirstBytes].Add(record);
            }
        }

        public int CountRecords(byte[] key)
        {
            if (avBase.ContainsKey(key))
                return avBase[key].Count;
            else
                return -1;
        }

        public AVRecord GetRecord(byte[] key, int number)
        {
            return avBase[key][number];
        }

        public int CountKey
        {
            get { return countKey; }
        }

        public bool ContainsKey(byte[] key)
        {
            return avBase.ContainsKey(key);
        }

    }



    class ByteComparer : IComparer<byte[]>
    {
        public int Compare(byte[] x, byte[] y)
        {
            int count = x.Count();
            if (count != y.Count())
                return 10;
            for (int i = 0; i < count; i++)
            {
                if (x[i] < y[i])
                    return -1;
                if (x[i] > y[i])
                    return 1;
            }
            return 0;
        }
    }





}
