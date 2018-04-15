using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Polly.Data;

namespace PollyAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Website> Websites { get; set; }

        public Website SelectedItem { get; set; }

        public MainWindow()
        {
            Websites = new ObservableCollection<Website>(DataAccess.GetWebsites());
            InitializeComponent();
            listBox.DataContext = Websites;
        }

        private void AddDataPoint_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
