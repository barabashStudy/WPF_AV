using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WpfAV
{
    class AVBase
    {
        protected string nameFile;
        protected uint recordCount;
        protected static string identifier = "AVBase";
        public AVBase(string nameFile)
        {
            this.nameFile = nameFile;
            recordCount = 0;
        }

    }







    class AVBaseWriter : AVBase
    {
        public AVBaseWriter(string nameFile) : base(nameFile) { }
        public bool Write(AVRecord record)
        {
            if (record == null)
                return false;
            if (!File.Exists(nameFile))
            {
                BinaryWriter writer = new BinaryWriter(File.Open(nameFile, FileMode.Create, FileAccess.Write));
                writer.Write(Encoding.Unicode.GetBytes(identifier));
                recordCount++;
                writer.Write(recordCount);
                writer.Close();
                FileStream fileStream = new FileStream(nameFile, FileMode.Append, FileAccess.Write);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, record);
                fileStream.Close();
            }
            else
            {
                BinaryReader reader = new BinaryReader(File.Open(nameFile, FileMode.Open, FileAccess.Read));
                string s = Encoding.Unicode.GetString(reader.ReadBytes(identifier.Count() * 2));
                if (s != identifier)
                {
                    reader.Close();
                    return false;
                }
                recordCount = reader.ReadUInt32();
                reader.Close();
                FileStream fileStream = new FileStream(nameFile, FileMode.Append, FileAccess.Write);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, record);
                fileStream.Close();
                recordCount++;
                BinaryWriter writer = new BinaryWriter(File.Open(nameFile, FileMode.Open, FileAccess.Write));
                writer.BaseStream.Position = identifier.Count() * 2;
                writer.Write(recordCount);
                writer.Close();
            }
            return true;
        }
    }

    class AVBaseReader : AVBase
    {
        private FileStream fileStream;
        private BinaryFormatter binaryFormatter;
        public AVBaseReader(string nameFile) : base(nameFile) { }

        public bool Open()
        {
            BinaryReader reader = new BinaryReader(File.Open(nameFile, FileMode.Open, FileAccess.Read));
            string s = Encoding.Unicode.GetString(reader.ReadBytes(identifier.Count() * 2));
            reader.Close();
            if (s != identifier)
                return false;
            fileStream = new FileStream(nameFile, FileMode.Open, FileAccess.Read);
            binaryFormatter = new BinaryFormatter();
            return true;
        }

        public bool Close()
        {
            if (fileStream != null)
                fileStream.Close();
            else
                return false;
            return true;
        }

        public AVRecord GetFirstRecord()
        {
            if (fileStream != null && fileStream.Position != fileStream.Length)
            {
                fileStream.Seek(identifier.Count() * 2 + sizeof(uint), SeekOrigin.Begin);
                return (AVRecord)binaryFormatter.Deserialize(fileStream);
            }
            return null;
        }

        public AVRecord GetNextRecord()
        {
            if (fileStream != null && fileStream.Position != fileStream.Length)
            {
                return (AVRecord)binaryFormatter.Deserialize(fileStream);
            }
            return null;
        }
    }





}
