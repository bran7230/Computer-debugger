using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string filesize { get; set; }
        public string fileimage { get; set; }

        public Window4()

        {
            InitializeComponent();

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FilePath))
            {
                Filepathbox.Text = FilePath;
                Filenamebox.Text = FileName;
                Sizebox.Text = filesize;

                // Get the system icon for the file
                var iconImageSource = FileIconHelper.GetFileIcon(FilePath, smallIcon: true);
                if (iconImageSource != null)
                {
                    Fileimage.Source = iconImageSource;
                }

            }
        }
    }
}
