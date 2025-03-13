using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using WindowsInput;



namespace WpfApp1
{
    public partial class MainWindow : Window
    {

        public ObservableCollection<ProcessInfo> Processes { get; set; }

        InputSimulator sim = new InputSimulator();//use for keystrokes
        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this; // Set DataContext to MainWindow instance
            Processes = new ObservableCollection<ProcessInfo>();
            dataGrid.ItemsSource = Processes;
            UpdateProcessList();//calling tasks

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5); // Refresh every 5 seconds
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateProcessList();
        }

        public void Button_Click(object sender, RoutedEventArgs e)//system scan, too lazy so used defender lol
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Open Test?", "Yes or No", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",//loads cmd
                    Arguments = "/c sfc /scannow",//runs system scan
                    Verb = "runas",//admin
                    UseShellExecute = true,
                };

                try
                {
                    Process.Start(startInfo);
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    System.Windows.MessageBox.Show("Error", "Task");
                }
            }
            else if (result == MessageBoxResult.No)
            {
                System.Windows.MessageBox.Show("Not opening", "Task");
            }
            else
            {
                System.Windows.MessageBox.Show("Canceling open", "Task");
            }
        }

        public void Macro_Click(object sender, RoutedEventArgs e)//misnamed , opens memory diagnostic tool
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",//loads cmd
                Arguments = "/c Mdsched.exe",//opens memory diagnostic tool
                Verb = "runas",//admin
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            try
            {
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }


        }



        public void Internet_Click(object sender, RoutedEventArgs e)//shows all connected ip addresses
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",//loads cmd
                Arguments = "/k \"netstat -anob | findstr ESTABLISHED\"",//finds all established connections
                Verb = "runas",//admin
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Normal // Ensure window is visible
            };

            try
            {
                Process.Start(startInfo);
            }
            catch (Win32Exception ex)
            {
                System.Windows.MessageBox.Show($"Failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void File_Click(object sender, RoutedEventArgs e)//scans file based on file path
        {
            string file = Interaction.InputBox("Enter file path of file you want to scan(Example C:\\users\\...)", "File Path", "C:\\");

            if (string.IsNullOrEmpty(file))
            {
                System.Windows.MessageBox.Show("No file path entered", "Error");
                return;
            }

            if (!System.IO.File.Exists(file))
            {
                System.Windows.MessageBox.Show("File not found", "Error");
                return;
            }


            ScanFile(file);

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

            Process process = new Process { StartInfo = psi };//starts process
            process.Start();
            process.WaitForExit();

            string result = process.StandardOutput.ReadToEnd();
            System.Windows.MessageBox.Show(result, "Scan Results");//results of scanning
        }
        public void Heat_Click(object sender, RoutedEventArgs e)//opens heat window
        {
            Window1 window = new Window1();
            window.Show();
        }


        public void Download_Click(object sender, RoutedEventArgs e)//download manager window
        {
            Window2 window = new Window2();
            window.Show();
        }

        private void UpdateProcessList()//process list
        {
            var processList = Process.GetProcesses().Select(p => new ProcessInfo
            {
                Process = p.ProcessName,
                Usage = GetProcessMemoryUsage(p)
            });

            Processes.Clear();
            foreach (var process in processList)//for every process adds it to the list
            {
                Processes.Add(process);
            }
        }

        private int GetProcessMemoryUsage(Process process)
        {
            try
            {
                return (int)(process.WorkingSet64 / 1024 / 1024); // Convert memory usage to MB
            }
            catch
            {
                return 0;
            }
        }

        public void Ai_Click(object sender, RoutedEventArgs e)//ai window
        {
            Ai window = new Ai();
            window.Show();

        }



        //macro window
        public void Macro_Click_1(object sender, RoutedEventArgs e)
        {
            Window3 window = new Window3();
            window.Show();
        }


    }
}
