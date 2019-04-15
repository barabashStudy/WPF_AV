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
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Security.Cryptography;

namespace WpfAV
{
    /// <summary>
    /// Логика взаимодействия для add_sign.xaml
    /// </summary>
    public partial class add_sign : Window
    {
        public add_sign()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)            //?????????????????????
            {
                string fileName = fileDialog.FileName;
                textBox_signaturePath.Text = fileName;

                //AVBaseReader baseReader = new AVBaseReader(fileName);
                //RecordsDictionary avBase = new RecordsDictionary();
                //if (baseReader.Open())
                //{
                //    AVRecord r = baseReader.GetFirstRecord();
                //    //listBox1.Items.Add(r.Name + " " + basePath);
                //    avBase.Add(r);
                //    while ((r = baseReader.GetNextRecord()) != null)
                //    {
                //        avBase.Add(r);
                //        //listBox1.Items.Add(r.Name + " " + basePath);
                //    }
                //    baseReader.Close();
                //    //scanEngine.AddBase(avBase);

                //    MessageBox.Show("База добавлена");
                //}
                //else
                //    MessageBox.Show("Ошибка");

            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (textBox_signaturePath.Text == ""
            || textBox_signatureBegin.Text == ""
            || textBox_signatureLength.Text == ""
            || textBox_name.Text == ""
            || textBox_offsetStart.Text == ""
            || textBox_offsetEnd.Text == ""
            || textBox_fileType.Text == ""
            //|| base.Text == ""
            || (Int32.Parse(textBox_signatureBegin.Text)) < 0
            || (Int32.Parse(textBox_signatureLength.Text)) <= 0
            || (Int32.Parse(textBox_offsetStart.Text)) < 0
            || (Int32.Parse(textBox_offsetEnd.Text)) < 0
            || (Int32.Parse(textBox_offsetStart.Text)) > (Int32.Parse(textBox_offsetEnd.Text)))
                MessageBox.Show("Проверьте ввод данных");
            else
            {
                string signaturePath = textBox_signaturePath.Text;
                uint beginSignature = UInt32.Parse(textBox_signatureBegin.Text);
                uint lenght = UInt32.Parse(textBox_signatureLength.Text);

                string virusName = textBox_name.Text;
                uint offsetStart = UInt32.Parse(textBox_offsetStart.Text);
                uint offsetEnd = UInt32.Parse(textBox_offsetEnd.Text);
                string fileType = textBox_fileType.Text;
                string basePath = "MY_BASE.bs";
                //basePathText.Text;

                if (File.Exists(signaturePath))
                {
                    BinaryReader reader = new BinaryReader(File.Open(signaturePath, FileMode.Open));
                    reader.BaseStream.Position = beginSignature;
                    byte[] firstBytes = reader.ReadBytes(4);
                    reader.BaseStream.Position = beginSignature;
                    byte[] text = reader.ReadBytes((int)lenght);
                    SHA1 sha1 = new SHA1CryptoServiceProvider();
                    byte[] hash = sha1.ComputeHash(text);
                    reader.Close();

                    if (firstBytes != null && hash != null)
                    {
                        AVRecord record = new AVRecord(virusName, offsetStart, offsetEnd, lenght, firstBytes, hash, fileType);
                        AVBaseWriter baseWriter = new AVBaseWriter(basePath);
                        if (!baseWriter.Write(record))
                            MessageBox.Show("Ошибка!");
                        this.Close();
                        MessageBox.Show("добавлен!");

                    }
                    else
                        MessageBox.Show("Ошибка!");
                }
                else
                    MessageBox.Show("Указанный файл с сигнатурой не существует!");
            }

        }

        private void textBox_signatureBegin_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
