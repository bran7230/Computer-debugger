using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;


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

            Searchtext.TextChanged += Searchtext_TextChanged;

        }



        private void Searchtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = Searchtext.Text;
            listBox.Items.Clear();

            foreach (FileInfo file in files)
            {
                if (file.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)) // Case-insensitive search
                {
                    listBox.Items.Add(file.Name);
                }
            }
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {


            if (listBox.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Please select a file from the list.");
            }

            if (listBox.SelectedItem != null)
            {
                try
                {
                    System.Windows.MessageBox.Show(dir.FullName + "\\" + listBox.SelectedItem.ToString());
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error opening file: " + ex.Message);
                }
            }



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
            {
                Window4 window4 = new Window4();
#pragma warning disable CS8604 // Possible null reference argument.
                string filePath = System.IO.Path.Combine(dir.FullName, listBox.SelectedItem.ToString());
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                string fileName = listBox.SelectedItem.ToString();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                string fileSize = string.Empty;
                try
                {
                    long fileSizeInBytes = new FileInfo(filePath).Length;
                    fileSize = (fileSizeInBytes / 1000).ToString();
                    window4.filesize = fileSize + " MB";
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

        public void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure you want to delete this file?", "Delete File", System.Windows.MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    string filePath = System.IO.Path.Combine(dir.FullName, listBox.SelectedItem.ToString());
#pragma warning restore CS8604 // Possible null reference argument.
                    try
                    {
                        File.Delete(filePath);
                        System.Windows.MessageBox.Show("File deleted successfully: " + filePath);
                        listBox.Items.Remove(listBox.SelectedItem);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Error deleting file: " + ex.Message);
                    }

                }

                else
                {
                    System.Windows.MessageBox.Show("Delete operation cancelled.");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please select a file from the list.");
            }
        }

        private void Scan_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Please select a file from the list.");
            }
            if (listBox.SelectedItem != null)
            {
                string filePath = System.IO.Path.Combine(dir.FullName, listBox.SelectedItem.ToString());
                ScanFile(filePath);
            }


        }

        private void ScanFile(string filePath)//actual filescan
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c \"\"C:\\Program Files\\Windows Defender\\MpCmdRun.exe\" -Scan -ScanType 3 -File {filePath}\"",//microsft defender scan
                RedirectStandardOutput = true,//redirects output to messagebox
                UseShellExecute = false,
                CreateNoWindow = true//so I dont have to see it
            };

            if (!File.Exists(filePath))
            {
                System.Windows.MessageBox.Show("File not found", "Error");
                return;
            }

            //Error cases for different file types

            if (filePath.Contains(".pdf"))
            {
                System.Windows.MessageBox.Show("File is a pdf, cannot scan", "Error");
                return;
            }

            if (filePath.Contains(".docx"))
            {
                System.Windows.MessageBox.Show("File is a word document, cannot scan", "Error");
                return;
            }

            if (filePath.Contains(".xlsx"))
            {
                System.Windows.MessageBox.Show("File is an excel document, cannot scan", "Error");
                return;
            }

            if (filePath.Contains(".pptx"))
            {
                System.Windows.MessageBox.Show("File is a powerpoint document, cannot scan", "Error");
                return;
            }

            if (filePath.Contains(".txt"))
            {
                System.Windows.MessageBox.Show("File is a text document, cannot scan", "Error");
                return;
            }

            if (filePath.Contains(".jpg"))
            {
                System.Windows.MessageBox.Show("File is a jpg, cannot scan", "Error");
                return;
            }

            if (filePath.Contains(".png"))
            {
                System.Windows.MessageBox.Show("File is a png, cannot scan", "Error");
                return;
            }

            if (filePath.Contains(".webp"))
            {
                System.Windows.MessageBox.Show("File is a webp, cannot scan", "Error");
                return;
            }

            if (filePath.Contains(".mp4"))
            {
                System.Windows.MessageBox.Show("File is a mp4, cannot scan", "Error");
                return;
            }

            Process process = new Process { StartInfo = psi };//starts process
            process.Start();
            process.WaitForExit();

            string result = process.StandardOutput.ReadToEnd();
            System.Windows.MessageBox.Show(result, "Scan Results");//results of scanning
        }
    }
}
