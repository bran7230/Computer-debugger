using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Threading;
using WindowsInput;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window3.xaml
    /// </summary>
    /// 

    public partial class Window3 : Window
    {
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        bool recording = false;
        bool macroStarted = false;
        bool keystroke = false;
        private Stopwatch stopwatch = new Stopwatch();
        InputSimulator sim = new InputSimulator();//use for keystrokes
        DispatcherTimer timer;
        string path = @"C:\Users\Public\Documents\keys.txt";
        public Window3()
        {
            InitializeComponent();
            this.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartKeyLogger();
        }

        private void StartKeyLogger()
        {
            // Start recording macro
            MessageBoxResult result = System.Windows.MessageBox.Show("Record Keys? Press F9 to Start", "Yes or No", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                recording = true;
                System.Windows.MessageBox.Show("Recording Keys. Press F9, and press F1 to Stop");
                this.KeyDown += OnKeyDownHandler;
            }

        }

        private void OnKeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (recording && e.Key == Key.F9)
            {
                System.Windows.MessageBox.Show("Recording Started");
                macroStarted = true;
            }
            else if (macroStarted && e.Key == Key.F1)
            {
                System.Windows.MessageBox.Show("Recording Stopped");
                recording = false;
                
            }

            if (recording && macroStarted)
            {
                Dictionary<int, bool> previousKeyStates = new Dictionary<int, bool>();
                if (keystroke == false)
                {
                    Task.Run(() =>
                    {
                        keystroke = true;
                        while (macroStarted)
                        {

                            for (int i = 0; i < 255; i++)
                            {
                                int keyState = GetAsyncKeyState(i);
                                bool isKeyDown = (keyState & 0x8000) != 0;

                                if (isKeyDown && (!previousKeyStates.ContainsKey(i) || !previousKeyStates[i]))
                                {
                                    string key = KeyInterop.KeyFromVirtualKey(i).ToString();
                                    Dispatcher.Invoke(() =>
                                    {
                                        // Ensure the file is created if it doesn't exist
                                        if (!System.IO.File.Exists(path))
                                        {
                                            System.IO.File.Create(path).Dispose();
                                        }
                                        // Append the key to the file
                                        System.IO.File.AppendAllText(path, key + " ");
                                    });
                                }

                                previousKeyStates[i] = isKeyDown;
                            }
                        }
                    });
                }
            }
        }

        private void Keyboard_Click(object sender, RoutedEventArgs e)
        {
            int timer_time;
            if (int.TryParse(Time.Text, out timer_time))
            {
                DispatcherTimer delayTimer = new DispatcherTimer();
                delayTimer.Interval = TimeSpan.FromSeconds(timer_time); // Use the parsed integer value
                delayTimer.Tick += DelayTimer_Tick;
                delayTimer.Start();
            }
            else
            {
                System.Windows.MessageBox.Show("Please enter a valid number for the timer.");
            }
        }

        private void DelayTimer_Tick(object sender, EventArgs e)
        {
            int spam_time;
            if (int.TryParse(End_Time.Text, out spam_time))
            {
                DispatcherTimer spamTimer = new DispatcherTimer();
                spamTimer.Interval = TimeSpan.FromMilliseconds(100); // Set interval for spamming text input
                spamTimer.Tick += SpamTimer_Tick;
                spamTimer.Start();
                DispatcherTimer stopTimer = new DispatcherTimer();
                stopTimer.Interval = TimeSpan.FromSeconds(10); // 10 seconds of spamming
                stopTimer.Tick += (s, ev) =>
                {
                    spamTimer.Stop();
                    stopTimer.Stop();
                };
                stopTimer.Start();
                ((DispatcherTimer)sender).Stop(); // Stop the delay timer
            }
            else
            {
                System.Windows.MessageBox.Show("Please enter a valid number for the spam timer.");
            }

        }

        private void SpamTimer_Tick(object sender, EventArgs e)
        {
            string textOutput = MacroTextBox.Text;
            sim.Keyboard.TextEntry(textOutput + " ");
        }
    }
}