using System;
using System.Collections.Generic;
using System.IO;
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
