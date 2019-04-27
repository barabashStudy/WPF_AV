using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.IO.Compression;

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
                if (Path.GetExtension(fullPath) == ".zip")
                {
                    string zipDir = GetDirFromZip(fullPath);
                    NotifyAboutAllFilesInDirectory(zipDir);
                }
                else
                {
                    AddFile(fullPath);
                    NotifyObservers(fullPath);
                }

                System.Windows.MessageBox.Show("Готово!");
            }
            else
            {

                NotifyAboutAllFilesInDirectory(fullPath);

                //DirectoryInfo dir = new DirectoryInfo(fullPath);
                //FileInfo[] filesInDir = dir.GetFiles("*", SearchOption.AllDirectories);
                //foreach (FileInfo file in filesInDir)
                //{
                //    string filePath = file.FullName;
                //    AddFile(filePath);
                //    NotifyObservers(filePath);
                //}

                System.Windows.MessageBox.Show("Готово!");

            }

            //engine.ModifyText(LB2);
            //engine.disp();
            //LB2.Items.Add("----------------");
        }

        private void NotifyAboutAllFilesInDirectory(string fullPath)
        {
            DirectoryInfo dir = new DirectoryInfo(fullPath);
            FileInfo[] filesInDir = dir.GetFiles("*", SearchOption.AllDirectories);
            foreach (FileInfo file in filesInDir)
            {
                string filePath = file.FullName;
                if (Path.GetExtension(filePath) == ".zip")
                {
                    string zipDir = GetDirFromZip(filePath);
                    NotifyAboutAllFilesInDirectory(zipDir);
                }
                else
                {
                    AddFile(filePath);
                    NotifyObservers(filePath);
                }
            }
        }
        private string GetDirFromZip(string zipPath)
        {
            string zipDir = Path.GetDirectoryName(zipPath) + '\\' + Path.GetFileNameWithoutExtension(zipPath);
            ZipFile.ExtractToDirectory(zipPath, zipDir);
            return zipDir;
        }
















    }
}
