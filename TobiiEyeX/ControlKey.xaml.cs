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

namespace TobiiEyeX {
    /// <summary>
    /// Interaction logic for ControlKey.xaml
    /// </summary>
    public partial class ControlKey : UserControl {

        public static readonly DependencyProperty TopLegendProperty = DependencyProperty.Register("CtrlTopLegend", typeof(string), typeof(ControlKey));

        public string CtrlTopLegend {
            get { return (string)GetValue(TopLegendProperty); }
            set { SetValue(TopLegendProperty, value); }
        }

        public ControlKey() {
            InitializeComponent();
            DataContext = this;
        }
    }
}
