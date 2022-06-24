using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Prozesssynchronisation
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }




        private readonly object syncLock = new object();
        private void Thread_safe_method_1()
        {
            lock(syncLock) 
            {
                /* critical code */
            }
        }



        private readonly Mutex m = new Mutex();
        private void Thread_safe_method_2()
        {
            m.WaitOne();
            try 
            {
                /* critical code */
            } 
            finally 
            {
                m.ReleaseMutex();
            }
        }




        Semaphore sem = new Semaphore(initialCount:0, maximumCount:2, name:@"Global\MySemaphoreName");
        private void Thread_safe_method_3()
        {
            sem.WaitOne();
            try 
            {
                /* critical code */
            } 
            finally 
            {
                sem.Release();
            }
        }
    }
}
