using Polly.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PollyAdmin
{
    /// <summary>
    /// Interaction logic for WebsiteOverview.xaml
    /// </summary>
    public partial class WebsiteOverview : UserControl
    {
        public Website Website { get; set; }

        public WebsiteOverview()
        {
            InitializeComponent();
        }
    }
}
