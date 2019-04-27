using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAV
{


    
    class VirDetectedWrite
    {

        //private MainWindow Form1 { get; set; }

        //public Class2(MainWindow f)
        //{
        //    Form1 = f;
        //}


        //public void SetItems()
        //{

        //    Form1.LB2.Items.Add("123");
        //}

        public void ModifyText(System.Windows.Controls.ListBox myLB)
        {
            foreach (var virus in vir.VirusesForWrite)
                myLB.Items.Add(virus);


        }


    }
}
