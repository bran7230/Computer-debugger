using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
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
        // Get the state of a key
        public static extern int GetAsyncKeyState(Int32 i);

        // Variables for keylogging
        bool recording = false;
        bool macroStarted = false;
        bool keystroke = false;

        //Spam keys for text input
        InputSimulator sim = new InputSimulator();
        DispatcherTimer timer;

        // Path to save the keys
        string path = @"C:\Users\Public\Documents\keys.txt";


        public Window3()
        {
            InitializeComponent();
            // Set the focus to the window
            this.Focus();
        }

        // Start the keylogger button
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
                // Start the keylogger
                this.KeyDown += OnKeyDownHandler;
            }

        }

        private void OnKeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //Record keys Pop up
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
                // Dictionary to hold the previous key states
                Dictionary<int, bool> previousKeyStates = new Dictionary<int, bool>();

                //If its not running, run it bool
                if (keystroke == false)
                {
                    Task.Run(() =>
                    {
                        //So it does not run again while running
                        keystroke = true;
                        // Loop through all the keys
                        while (macroStarted)
                        {

                            for (int i = 0; i < 255; i++)
                            {
                                // Get the state of the key
                                int keyState = GetAsyncKeyState(i);

                                // Check if the key is down
                                bool isKeyDown = (keyState & 0x8000) != 0;

                                //Logic to record keys if key is pressed, and its not duped for keys
                                if (isKeyDown && (!previousKeyStates.ContainsKey(i) || !previousKeyStates[i]))
                                {
                                    //Converts keycode to string, ex W, A, S, D
                                    string key = KeyInterop.KeyFromVirtualKey(i).ToString();

                                    //starting the dispatcher for File saving keys
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

        //Text spam Click method
        private void Keyboard_Click(object sender, RoutedEventArgs e)
        {
            int timer_time;
            // Parse the text input for the timer, ex 5 would be a Timer for 5 seconds
            if (int.TryParse(Time.Text, out timer_time))
            {
                // Start the delay timer
                DispatcherTimer delayTimer = new DispatcherTimer();
                delayTimer.Interval = TimeSpan.FromSeconds(timer_time); // Use the parsed integer value
                delayTimer.Tick += DelayTimer_Tick;
                delayTimer.Start();
            }

            // If the input is not a valid number, put a message box
            else
            {
                System.Windows.MessageBox.Show("Please enter a valid number for the timer.");
            }
        }

        //Delay for timer method
        private void DelayTimer_Tick(object sender, EventArgs e)
        {
            //How long it will spam for
            int spam_time;
            // Parse the text input for the timer, ex 5 would be a Timer for 5 seconds
            if (int.TryParse(End_Time.Text, out spam_time))
            {
                DispatcherTimer spamTimer = new DispatcherTimer();
                spamTimer.Interval = TimeSpan.FromMilliseconds(100); // Set interval for spamming text input
                spamTimer.Tick += SpamTimer_Tick;
                spamTimer.Start();
                DispatcherTimer stopTimer = new DispatcherTimer();

                // Set the interval for the spam timer for it to stop
                stopTimer.Interval = TimeSpan.FromSeconds(spam_time); 

                // Stop the spam timer after _ seconds ex 10 seconds
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
            // Get the text from the TextBox to spam
            string textOutput = MacroTextBox.Text;

            // Spam the text where the user is clicking
            sim.Keyboard.TextEntry(textOutput + " ");
        }
    }
}