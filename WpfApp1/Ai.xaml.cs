using System;
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

            return input switch
            {
                string s when s.Contains("error") => "It seems you're encountering an error. Can you provide more details?(Software, Hardware)",
                string s when s.Contains("software") => "What software are you having trouble with?",
                string s when s.Contains("hardware") => "What hardware are you having trouble with?",
                string s when s.Contains("windows") => "Want to try Basic Commands?",
                string s when s.Contains("basic commands") => "Ok, Want a full System scan type Full system scan?",
                string s when s.Contains("full System scan") => "Ok, I will start the scan",
                string s when s.Contains("debug") => "Have you tried checking the debugger or adding some breakpoints?",
                string s when s.Contains("help") => "error for error help, debug for debug help",
                _ => "I'm not sure how to respond to that. Could you please rephrase? or Type Help"
            };


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
    }
}

