using System;
using System.Collections.Generic;
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
        InputSimulator sim = new InputSimulator();//use for keystrokes
        DispatcherTimer timer;
        public Window3()
        {
            InitializeComponent();
         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartMacroRecord();
        }

        private void StartMacroRecord()
        {
            // Start recording macro

            
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
            if(int.TryParse(End_Time.Text, out spam_time))
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
            sim.Keyboard.TextEntry(textOutput +" ");
        }
    }
}
