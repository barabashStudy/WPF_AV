using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAV
{
    class Print_virus : IObserver<VirusInfo>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(VirusInfo value)
        {

            var Wins = System.Windows.Application.Current.Windows;


            //MainWindow.GetWindow(System.Windows.DependencyObject )
            //MY_LB(MainWindow.GetWindow(MainWindow.))
        }

        public void test()
        {

            var Wins = System.Windows.Application.Current.Windows;

            //Wins[0].

            //MainWindow.GetWindow(System.Windows.DependencyObject )
            //MY_LB(MainWindow.GetWindow(MainWindow.))
        }







        public void MY_LB(System.Windows.Controls.ListBox myLB)
        {
            //foreach (var virus in Viruses)
                //myLB.Items.Add(virus.VirusName + "   " + virus.Path);
        }

        public void disp()
        {
            //observers.Clear();
            //Viruses.Clear();

        }



    }
}










