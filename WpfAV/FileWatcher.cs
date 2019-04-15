using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace WpfAV
{
    /// <summary>
    /// класс, отслеживающий изменения в директориях
    /// (поставщик сообщений)
    /// </summary>
    class FileWatcher : FileObservable
    {
        private List<string> directories;
        private List<FileSystemWatcher> watchers;
        public FileWatcher()
        {
            directories = new List<string>();
            watchers = new List<FileSystemWatcher>();
        }

        private void AddDirectoryWithoutScan(string path)
        {
            if (!directories.Contains(path))
            {
                directories.Add(path);
                FileSystemWatcher watcher = new FileSystemWatcher(path);
                watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName
                                       | NotifyFilters.DirectoryName;
                watcher.Created += new FileSystemEventHandler(OnChanged);
                watcher.Changed += new FileSystemEventHandler(OnChanged);
                watcher.Renamed += new RenamedEventHandler(OnRenamed);
                watcher.EnableRaisingEvents = true;
                watchers.Add(watcher);
            }
        }

        public bool AddDirectory(string path)
        {
            if (!directories.Contains(path))
            {
                FileFinder fileFinder = new FileFinder(path);
                foreach (var observer in observers)
                    fileFinder.Subscribe(observer);
                fileFinder.Start();

                AddDirectoryWithoutScan(path);

                string[] dirs = Directory.GetDirectories(path, "*",
                                                         SearchOption.AllDirectories);
                foreach (string dir in dirs)
                    AddDirectoryWithoutScan(dir);
                return true;
            }
            return false;
        }

        private void OnChanged(object sourse, FileSystemEventArgs e)
        {
            string path = e.FullPath;
            if (Directory.Exists(path) && e.ChangeType == WatcherChangeTypes.Created)
            {
                AddDirectory(path);
            }
            else if (File.Exists(path))
            {
                NotifyObservers(path);
            }
        }

        private void OnRenamed(object sourse, RenamedEventArgs e)
        {
            string path = e.FullPath;
            if (Directory.Exists(path))
            {
                AddDirectoryWithoutScan(path);
                directories.Remove(e.OldFullPath);
                bool flag = true;
                for (int i = 0; i < watchers.Count && flag; i++)
                {
                    if (watchers[i].Path == e.OldFullPath)
                    {
                        watchers.RemoveAt(i);
                        flag = false;
                    }
                }
            }
        }
    }
}
