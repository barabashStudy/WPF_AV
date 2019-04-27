using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Runtime.Serialization.Formatters.Binary;
using WinForms = System.Windows.Forms;
//using System.Windows.Forms;
using Path = System.IO.Path;
using System.Windows.Interop;

namespace WpfAV
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window , IObserver<VirusInfo>
    {


        public ListBox GETLB()
        {
             return LB2; 
        }


        public MainWindow()
        {
            InitializeComponent();
        }




        public void OnCompleted()
        {
            string curTimeLong = DateTime.Now.ToLongTimeString();
            Application.Current.Dispatcher.Invoke(new System.Action(() => LB2.Items.Add(curTimeLong + " \t| SCAN COMPLETED" )));
            //throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(VirusInfo value)
        {

            string curTimeLong = DateTime.Now.ToLongTimeString();
            Application.Current.Dispatcher.Invoke(new System.Action(() => LB2.Items.Add(curTimeLong + " \t| " + value.VirusName + " \t| " + value.Path)));
        }




        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //tb1.Text = "heh"; 

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            add_sign addSign = new add_sign();
            addSign.ShowDialog();

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)            //?????????????????????
            {
                string fileName = fileDialog.FileName;
                //TB_filePath.Text = fileName;

                AVBaseReader baseReader = new AVBaseReader(fileName);
                RecordsDictionary avBase = new RecordsDictionary();
                if (baseReader.Open())
                {
                    AVRecord r = baseReader.GetFirstRecord();
                    //listBox1.Items.Add(r.Name + "\t" + fileName);
                    avBase.Add(r);
                    while ((r = baseReader.GetNextRecord()) != null)
                    {
                        avBase.Add(r);
                        //listBox1.Items.Add(r.Name + "\t" + fileName);
                    }
                    baseReader.Close();
                    engine.AddBase(avBase);


                    MessageBox.Show("База добавлена");
                }
                else
                    MessageBox.Show("Ошибка");

            }



            //add_base addBase = new add_base();
            //addBase.ShowDialog();

        }



        ScanEngine engine = new ScanEngine();
        FilePreparer preparer = new FilePreparer();


        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {

            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                string fileName = fileDialog.FileName;

                //-----------------------------------------------------------------------------------
                FileFinder finder = new FileFinder(fileName);
                //FilePreparer preparer = new FilePreparer();

                engine.Subscribe(mainW);

                finder.Start();

                preparer.Subscribe(finder);
                preparer.Subscribe(engine);

            }

        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            engine.disp();

            WinForms.FolderBrowserDialog folderBrowser = new WinForms.FolderBrowserDialog();

            WinForms.DialogResult result = folderBrowser.ShowDialog();

            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                engine.Subscribe(mainW);

                FileWatcher fileWatcher = new FileWatcher();

                //fileWatcher.Subscribe(finder);
                fileWatcher.Subscribe(preparer);

                fileWatcher.AddDirectory(folderBrowser.SelectedPath);

                

                //------------------------------------------------------------
                //fileWatcher.AddFile()
                //preparer.Subscribe(fileWatcher);
                //engine
                //preparer.Subscribe(finder);
                //preparer.Subscribe(finder);
                // preparer.Subscribe(engine);
                //fileWatcher.Subscribe(engine);
            }
        }





        public ListBox GetLB()
        {
             return LB2; 

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog folderBrowser = new WinForms.FolderBrowserDialog();

            WinForms.DialogResult result = folderBrowser.ShowDialog();

            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {

                FileFinder finder = new FileFinder(folderBrowser.SelectedPath);
                FilePreparer preparer = new FilePreparer();

                engine.Subscribe(mainW);

                finder.Start();

                preparer.Subscribe(finder);
                preparer.Subscribe(engine);
            }


        }
    }


    public static class vir
    {
        
        public static List<VirusInfo> VirusesForWrite;
        //public static 
        //public static VirusInfo virInfo;
    }

    

}
