using System.ComponentModel;

public class ProcessInfo : INotifyPropertyChanged
{
    private string process = string.Empty;
    public string Process
    {
        get { return process; }
        set
        {
            if (process != value)
            {
                process = value;
                OnPropertyChanged(nameof(Process));
            }
        }
    }

    private int usage;
    public int Usage
    {
        get { return usage; }
        set
        {
            if (usage != value)
            {
                usage = value;
                OnPropertyChanged(nameof(Usage));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
