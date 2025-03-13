using LibreHardwareMonitor.Hardware;
using System.Collections.ObjectModel;
using System.Windows;


namespace WpfApp1
{
    public partial class Window1 : Window
    {
        private System.Timers.Timer _timer; // Specify System.Timers.Timer
        private SystemInfoViewModel _viewModel; // Add this line to declare _viewModel

        public Window1()
        {
            InitializeComponent();

            _viewModel = new SystemInfoViewModel();//info
            DataContext = _viewModel;

            _timer = new System.Timers.Timer(1000); // Update every second
            _timer.Elapsed += (s, e) => Dispatcher.Invoke(() => _viewModel.UpdateSensorData());
            _timer.Start();
        }

        protected override void OnClosed(EventArgs e)
        {
            _viewModel.Dispose();
            _timer.Dispose();
            base.OnClosed(e);
        }
    }

    public class SensorData
    {
        public required string Component { get; set; }//getting component for data
        public required string Sensor { get; set; }//sensor type
        public double? Value { get; set; }
        public required string Type { get; set; }
    }

    public class SystemInfoViewModel : IDisposable
    {
        private readonly Computer _computer;
        public ObservableCollection<SensorData> SensorDataCollection { get; }

        public SystemInfoViewModel()
        {
            _computer = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMemoryEnabled = true,
                IsMotherboardEnabled = true,
                IsStorageEnabled = true,
                IsNetworkEnabled = true,
                IsBatteryEnabled = true
            };

            _computer.Open();
            SensorDataCollection = new ObservableCollection<SensorData>();
        }

        public void UpdateSensorData()
        {
            try
            {
                SensorDataCollection.Clear();

                foreach (var hardware in _computer.Hardware)
                {
                    hardware.Update();
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature && sensor.Value.HasValue)
                        {
                            AddSensorData(
                                hardware.Name,
                                sensor.Name,
                                sensor.Value.Value,
                                sensor.SensorType.ToString()
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error reading sensors: {ex.Message}");
            }
        }

        private void AddSensorData(string component, string sensorName, double value, string type)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                SensorDataCollection.Add(new SensorData
                {
                    Component = component,
                    Sensor = sensorName,
                    Value = value,
                    Type = type
                });
            });
        }

        public void Dispose()
        {
            _computer.Close();
            _computer.Reset();
        }
    }
}