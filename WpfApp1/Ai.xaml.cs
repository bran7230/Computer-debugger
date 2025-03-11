using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Management;
using System.Text;
using Microsoft.VisualBasic;
using System.DirectoryServices;


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
            MessagesListBox.ItemsSource = _messages;

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
            else if(input == "ram scan")
            {
                ramScan(); // Call the method to start the ram scan
            }

            else if (input == "device manager")
            {
                deviceManager(); 
            }

            else if(input == "disk cleanup")
            {
                diskclean();
            }

            else if(input =="temp file clean")
            {
                tempfileclean();
            }
            else if(input == "disk manager")
            {   
                diskManage();
            }

            else if(input == "view storage")
            {
                viewStorage();
            }

            else if(input == "control panel")
            {
                Process.Start("control.exe");
            }

            else if(input == "driver update")
            {
                driverUpdate();
            }


                return input switch
                {
                    string s when s.Contains("error") => "It seems you're encountering an error. Can you provide more details?(Software, Hardware)",
                    string s when s.Contains("hardware") => "What hardware are you having trouble with?",
                    string s when s.Contains("software") => "Want to try Basic Commands(Type Basic Commands) or Advanced?",
                    string s when s.Contains("basic commands") => "Ok, Full system scan, Ram scan, Device Manager, Disk Cleanup, Temp file clean",
                    string s when s.Contains("full System scan") => "Ok, I will start the scan",
                    string s when s.Contains("debug") => "Have you tried checking the debugger or adding some breakpoints?",
                    string s when s.Contains("help") => "error for error help, debug for debug help",
                    string s when s.Contains("thank you") => "You're welcome! Feel free to ask if you need more help.",
                    string s when s.Contains("ram scan") => "Starting RAM scan...",
                    string s when s.Contains("device manager") => "Opening Device Manager...",
                    string s when s.Contains("advanced") => "View storage, Disk manager, Control panel, Driver update",
                    string s when s.Contains("disk cleanup") => "Starting Disk Cleanup...",
                    string s when s.Contains("temp file clean") => "Starting Temp File Cleanup...",
                    string s when s.Contains("view storage") => "Opening Storage Viewer...",
                    string s when s.Contains("control panel") => "Opening Control Panel...",
                    string s when s.Contains("driver update") => "Opening Driver Updater",
               
                    _ => "I'm not sure how to respond to that. Could you please rephrase? or Type Help"
                };


        }

        private void driverUpdate()
        {



        }


        private void viewStorage()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",//loads cmd
                Arguments = "/c start ms-settings:storagesense",
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
                Arguments = "/c cleanmgr /sagerun:1",
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
                Arguments = "/c cleanmgr",
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
        public string Sender { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string DisplayText => $"{(Sender == "AI" ? "Assistant" : "User")}: {Text}";
    }
}

