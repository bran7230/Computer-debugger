using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;



namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Ai.xaml
    /// </summary>
    public partial class Ai : Window
    {
        // Observable collection to hold our chat messages
        private ObservableCollection<ChatMessage> _messages;


        public Ai()
        {
            InitializeComponent();
            _messages = new ObservableCollection<ChatMessage>();
            MessagesListBox.ItemsSource = _messages;// Bind the ListBox to the collection

            // Add a welcome message from the AI
            _messages.Add(new ChatMessage { Sender = "AI", Text = "Hello! How can I help you today?(Type help for options)" });
        }

        // Click event for the Send button
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userInput = UserInputTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(userInput))
                return;

            // Add the user's message to the conversation
            _messages.Add(new ChatMessage { Sender = "User", Text = userInput });

            // Get a hardcoded response based on the user input
            string response = GetResponse(userInput);

            // Add the AI's response to the conversation
            _messages.Add(new ChatMessage { Sender = "AI", Text = response });

            // Clear the input TextBox
            UserInputTextBox.Clear();
        }

        // A simple rule-based method to generate a response based on keywords
        private string GetResponse(string userInput)
        {
            // Convert input to lower case for easier matching
            var input = userInput.ToLower();

            if (input == "full system scan")
            {   // Call the method to start the full system scan
                StartFullSystemScan();
                return "Full system scan initiated. Please wait for the results.";
            }
            else if (input == "ram scan")
            {
                ramScan(); 
            }

            else if (input == "device manager")
            {
                deviceManager();
            }

            else if (input == "disk cleanup")
            {
                diskclean();
            }

            else if (input == "temp file clean")
            {
                tempfileclean();
            }
            else if (input == "disk manager")
            {
                diskManage();
            }

            else if (input == "view storage")
            {
                viewStorage();
            }

            else if (input == "control panel")
            {
                Process.Start("control.exe");//opens control panel
            }

            else if (input == "event viewer")
            {
                openEventviewer();
            }

            else if (input == "problem reports")
            {
                openProblemreports();
            }

            else if (input == "system information")
            {
                Process.Start("msinfo32.exe");//opens system information
            }
            else if (input == "directx")
            {
                Process.Start("dxdiag.exe");//opens directx
            }
            else if (input == "ipconfig")
            {
                ShowNetworkConfig();
            }

            else if (input == "performance monitor")
            {
                openPerformance();
            }

            else if (input == "resource monitor")
            {
                openResource();
            }

            else if (input == "disk check")
            {
                diskCheck();
            }



            //Return a response based on the user input Switch cases, add more Future Me if you want to add more commands
            return input switch
            {
                string s when s.Contains("help") => "Want to try Basic Commands(Type Basic Commands) or Advanced?",
                string s when s.Contains("basic commands") => "Full system scan, Ram scan, Device Manager, Disk Cleanup, Temp file clean, System information, Performance Monitor,",
                string s when s.Contains("full System scan") => "Ok, I will start the scan",
                string s when s.Contains("thank you") => "You're welcome! Feel free to ask if you need more help.",
                string s when s.Contains("ram scan") => "Starting RAM scan...",
                string s when s.Contains("device manager") => "Opening Device Manager...",
                string s when s.Contains("advanced") => "View storage, Disk manager, Control panel, Event viewer, Problem reports, Directx, Ipconfig, Resource Monitor, Disk check",
                string s when s.Contains("disk cleanup") => "Starting Disk Cleanup...",
                string s when s.Contains("temp file clean") => "Starting Temp File Cleanup...",
                string s when s.Contains("view storage") => "Opening Storage Viewer...",
                string s when s.Contains("control panel") => "Opening Control Panel...",
                string s when s.Contains("event viewer") => "Opening Event Viewer...",
                string s when s.Contains("problem reports") => "Opening Problem Reports...",
                string s when s.Contains("system information") => "Opening System Information...",
                string s when s.Contains("directx") => "Opening DirectX...",
                string s when s.Contains("ipconfig") => "Showing Network Configuration...",
                string s when s.Contains("performance monitor") => "Opening Performance Monitor...",
                string s when s.Contains("resource monitor") => "Opening Resource Monitor...",
                string s when s.Contains("disk check") => "Starting Disk Check...",
                //Default not found case
                _ => "I'm not sure how to respond to that. Could you please rephrase? or Type Help"
            };
        }

        private void diskCheck()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",//loads cmd
                Arguments = "/c chkdsk",//opens disk check
                Verb = "runas",//admin
                UseShellExecute = true,

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
        private void openResource()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "resmon.exe",
                UseShellExecute = true
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

        private void openPerformance()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "perfmon.exe",
                UseShellExecute = true
            };
            try
            {
                Process.Start(startInfo);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            try
            {
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void ShowNetworkConfig()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/k ipconfig /all",
                UseShellExecute = true,
                Verb = "runas",
                WindowStyle = ProcessWindowStyle.Normal
            };
            try
            {
                var process = Process.Start(startInfo);

            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }

        private void openProblemreports()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "eventvwr.exe",
                UseShellExecute = true,

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

        private void openEventviewer()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",//loads cmd
                Arguments = "/c eventvwr.msc",//opens event viewer
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
        private void viewStorage()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",//loads cmd
                Arguments = "/c start ms-settings:storagesense",//opens storage settings
                Verb = "runas",//admin
                UseShellExecute = true,
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

        private void diskManage()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe", // loads cmd
                Arguments = "/c diskmgmt.msc", // command to open Disk Management
                Verb = "runas", // admin
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Normal
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

        private void tempfileclean()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",//loads cmd
                Arguments = "/c cleanmgr /sagerun:1",//clean temp files
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

        private void diskclean()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",//loads cmd
                Arguments = "/c cleanmgr",//opens disk cleanup
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

        private void deviceManager()
        {

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",//loads cmd
                Arguments = "/c devmgmt.msc",//opens memory diagnostic tool
                Verb = "runas",//admin
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden//hides the window
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

        private void ramScan()
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

        private void StartFullSystemScan()
        {

            System.Windows.MessageBox.Show("Starting full system scan...");
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",//loads cmd
                Arguments = "/c sfc /scannow",//runs system scan
                Verb = "runas",//admin
                UseShellExecute = true
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
    }


    // Simple class to hold each chat message
    public class ChatMessage
    {
        // Properties for the sender and text of the message
        public string Sender { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;

        // A read-only property to display the sender and text in the ListBox
        public string DisplayText => $"{(Sender == "AI" ? "Assistant" : "User")}: {Text}";
    }
}

