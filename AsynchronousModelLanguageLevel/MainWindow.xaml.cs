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

namespace AsynchronousModelLanguageLevel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnAsyncTask_Click(object sender, RoutedEventArgs e)
        {
            txtPrintArea.Text += "I am in thread: " + Thread.CurrentThread.ManagedThreadId + Environment.NewLine;
            var managedThreadId = await Task.Run(() => DoAsyncWork());
            txtPrintArea.Text += "Task done in thread: " + managedThreadId + Environment.NewLine;
            txtPrintArea.Text += "Now I am in thread: " + Thread.CurrentThread.ManagedThreadId + Environment.NewLine;
        }

        private int DoAsyncWork()
        {
            Thread.Sleep(5000);
            return Thread.CurrentThread.ManagedThreadId;
        }

        private void btnPrintThreadId_Click(object sender, RoutedEventArgs e)
        {
            txtPrintArea.Text += "Main Thread Id: " + Thread.CurrentThread.ManagedThreadId + Environment.NewLine;
        }
    }
}
