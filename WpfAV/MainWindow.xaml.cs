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

namespace WpfAV
{
    
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public ListBox GETLB()
        {
             return LB2; 
        }


        public MainWindow()
        {
            InitializeComponent();
        }


        


        //ListBox SomeProperty
        //{
        //    get
        //    {
        //        return LB2;
        //    }
        //    set
        //    {
        //        LB2 = value;
        //    }
        //}




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

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {

            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                string fileName = fileDialog.FileName;

                //-----------------------------------------------------------------------------------
                FileFinder finder = new FileFinder(fileName);
                //ScanEngine engine = new ScanEngine();
                FilePreparer preparer = new FilePreparer();
                //FileWatcher watcher = new FileWatcher();

                //engine.AddBase(avBase);
                preparer.Subscribe(engine);
                finder.AddFile(fileName);

                preparer.Subscribe(finder);

                //finder.Start();
                //-----------------------------------------------------------------------------------
                engine.ModifyText(LB2);
                engine.disp();
                LB2.Items.Add("----------------");

            }

        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            //VirDetectedWrite VIR1 = new VirDetectedWrite();
            //VIR1.ModifyText(LB2);

            engine.ModifyText(LB2);

        }





        public ListBox GetLB()
        {
             return LB2; 

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            WinForms.FolderBrowserDialog folderBrowser = new WinForms.FolderBrowserDialog();

            WinForms.DialogResult result = folderBrowser.ShowDialog();

            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                //string[] files = Directory.GetFiles(folderBrowser.SelectedPath);

                //-----------------------------------------------------------------------------------
                FileFinder finder = new FileFinder(folderBrowser.SelectedPath);
                //ScanEngine engine = new ScanEngine();
                FilePreparer preparer = new FilePreparer();

                //engine.AddBase(avBase);
                //finder.AddFile(folderBrowser.SelectedPath);

                finder.Start();

                preparer.Subscribe(finder);
                preparer.Subscribe(engine);



                //-----------------------------------------------------------------------------------
                engine.ModifyText(LB2);
                engine.disp();
                LB2.Items.Add("----------------");


            }





        }
    }


    public static class vir
    {
        
        public static List<VirusInfo> VirusesForWrite;
        //public static VirusInfo virInfo;
    }

    

}
