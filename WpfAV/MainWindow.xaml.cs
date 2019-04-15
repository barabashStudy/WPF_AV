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


namespace WpfAV
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
            add_base addBase = new add_base();
            addBase.ShowDialog();

        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            string fileName = fileDialog.FileName;
            textBox_aimFolder.Text = fileName;

            FileFinder finder = new FileFinder(fileName);
            FilePreparer preparer = new FilePreparer();
            preparer.Subscribe(finder);
            ScanEngine scanEngine = new ScanEngine();
            preparer.Subscribe(scanEngine);



            string basePath = "MY_BASE.bs";
            AVBaseReader baseReader = new AVBaseReader(basePath);
            RecordsDictionary avBase = new RecordsDictionary();
            if (baseReader.Open())
            {
                AVRecord r = baseReader.GetFirstRecord();
                listBox1.Items.Add(r.Name + " " + basePath);
                avBase.Add(r);
                while ((r = baseReader.GetNextRecord()) != null)
                {
                    avBase.Add(r);
                    listBox1.Items.Add(r.Name + " " + basePath);
                }
                baseReader.Close();
                scanEngine.AddBase(avBase);

                MessageBox.Show("База добавлена");
            }
            else
                MessageBox.Show("Ошибка");


            finder.Start();

        }
    }


    public static class vir
    {
        public static VirusInfo virInfo;
    }

}
