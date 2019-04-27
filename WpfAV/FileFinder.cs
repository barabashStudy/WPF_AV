using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace WpfAV
{
    /// <summary>
    /// класс для поиска файлов
    /// </summary>
    class FileFinder : FileObservable   //поставщик сообщений
    {
        private string fullPath;
        public FileFinder(string path)
        {
            fullPath = path;
        }

        public void Start()
        {
            Thread thread = new Thread(GetAllFiles);
            thread.Start();
        }
        private void GetAllFiles()
        {
            if (File.Exists(fullPath))
            {
                AddFile(fullPath);

                NotifyObservers(fullPath);

                System.Windows.MessageBox.Show("Сканирование окончено!");

            }
            else
            {
                DirectoryInfo dir = new DirectoryInfo(fullPath);
                FileInfo[] filesInDir = dir.GetFiles("*", SearchOption.AllDirectories);
                foreach (FileInfo file in filesInDir)
                {
                    string filePath = file.FullName;
                    AddFile(filePath);
                    NotifyObservers(filePath);
                }

                System.Windows.MessageBox.Show("Сканирование окончено!");

            }


            //engine.ModifyText(LB2);
            //engine.disp();
            //LB2.Items.Add("----------------");


        }
    }
}
