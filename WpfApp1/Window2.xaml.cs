using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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
#pragma warning disable CS8604 // Possible null reference argument.
                string filePath = System.IO.Path.Combine(dir.FullName, listBox.SelectedItem.ToString());
                
#pragma warning restore CS8604 // Possible null reference argument.
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

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {   Window4 window4 = new Window4();
                string filePath = System.IO.Path.Combine(dir.FullName, listBox.SelectedItem.ToString());
                string fileName = listBox.SelectedItem.ToString();
                string fileSize = string.Empty;
                try
                {
                    long fileSizeInBytes = new FileInfo(filePath).Length;
                    fileSize = (fileSizeInBytes / 1000).ToString(); 
                    window4.filesize = fileSize +" MB";
                }
                catch (IOException)
                {
                    // Handle exception
                }
                var fileInfo = new FileInfo(filePath);
                DateTime createTime = fileInfo.CreationTime;
                DateTime lastTime = fileInfo.LastWriteTime;
                FileAttributes fileAttributes = fileInfo.Attributes;
                // Open Window4 and pass filePath
              
                window4.FilePath = filePath;
#pragma warning disable CS8601 // Possible null reference assignment.
                window4.FileName = fileName;
#pragma warning restore CS8601 // Possible null reference assignment.

                window4.Create_time.Text = createTime.ToString();
                window4.Access_time.Text = lastTime.ToString();
                window4.Other_info.Text = fileAttributes.ToString();

                window4.Show();
            }
            else
            {
                System.Windows.MessageBox.Show("Please select a file from the list.");
            }
        }

    }
}
