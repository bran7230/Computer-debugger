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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string searchText = Searchtext.Text; // Fix the error by assigning the TextBox's Text property to a string variable
            listBox.Items.Clear();

            foreach (FileInfo file in files)
            {
                if (file.Name.Contains(searchText))
                {
                    listBox.Items.Add(file.Name);
                }
            }
        }
    }
}
