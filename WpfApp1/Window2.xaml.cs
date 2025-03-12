using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        DirectoryInfo dir;
        FileInfo[] files;
        string userName = Environment.UserName;
        public Window2()
        {
            InitializeComponent();
            dir = new DirectoryInfo(@"C:\Users\" + userName + "\\Downloads");
            files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                listBox.Items.Add(file.Name);
            }

        }

       

        private void button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show(dir.FullName + "\\" + listBox.SelectedItem.ToString());
          
        }

     private void File_open_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                string filePath = System.IO.Path.Combine(dir.FullName, listBox.SelectedItem.ToString());
                try
                {
                    var processInfo = new ProcessStartInfo(filePath)
                    {
                        UseShellExecute = true
                    };
                    Process.Start(processInfo);
                    System.Windows.MessageBox.Show("File opened successfully: " + filePath);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error opening file: " + ex.Message);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please select a file from the list.");
            }
        }
    }
}
