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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        DirectoryInfo dir;
        FileInfo[] files;
        string userName = Environment.UserName;
        public Window2()
        {
            InitializeComponent();
            dir = new DirectoryInfo(@"C:\Users\" + userName + "\\Downloads");
            files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                listBox.Items.Add(file.Name);
            }

        }

       

        private void button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show(dir.FullName + "\\" + listBox.SelectedItem.ToString());
          
        }

     
    }
}
