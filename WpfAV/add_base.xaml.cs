using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using System.Windows.Shapes;

namespace WpfAV
{
    /// <summary>
    /// Логика взаимодействия для add_base.xaml
    /// </summary>
    public partial class add_base : Window
    {
        public add_base()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)            //?????????????????????
            {
                string fileName = fileDialog.FileName;
                TB_filePath.Text = fileName;

                AVBaseReader baseReader = new AVBaseReader(fileName);
                RecordsDictionary avBase = new RecordsDictionary();
                if (baseReader.Open())
                {
                    AVRecord r = baseReader.GetFirstRecord();
                    listBox1.Items.Add(r.Name + "\t" + fileName);
                    avBase.Add(r);
                    while ((r = baseReader.GetNextRecord()) != null)
                    {
                        avBase.Add(r);
                        listBox1.Items.Add(r.Name + "\t" + fileName);
                    }
                    baseReader.Close();
                    //scanEngine.AddBase(avBase);


                    MessageBox.Show("База добавлена");
                }
                else
                    MessageBox.Show("Ошибка");

            }

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //myTable.Columns.Add("n1");
            //myTable.Columns.Add("n2");
            //foreach (DataColumn col in myTable.Columns)
            //{
            //    dataGrid1.Columns.Add(
            //        new DataGridTextColumn()
            //        {
            //            Header = col.ColumnName,
            //            Binding = new Binding(String.Format("[{0}]",
            //                col.ColumnName))
            //        });
            //}
            //dataGrid1.DataContext = myTable;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }




    public class Item
    {
        public string Path { get; set; }
        public string Name { get; set; }
    }



}
