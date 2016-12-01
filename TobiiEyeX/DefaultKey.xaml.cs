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
    /// Interaction logic for DefaultKey.xaml
    /// </summary>
    public partial class DefaultKey : UserControl {

        public static readonly DependencyProperty TopLegendProperty = DependencyProperty.Register("TopLegend", typeof(string), typeof(DefaultKey));
        public static readonly DependencyProperty BotLegendProperty = DependencyProperty.Register("BotLegend", typeof(string), typeof(DefaultKey));

        public string TopLegend {
            get { return (string)GetValue(TopLegendProperty); }
            set { SetValue(TopLegendProperty, value); }
        }

        public string BotLegend {
            get { return (string)GetValue(BotLegendProperty); }
            set { SetValue(BotLegendProperty, value); }
        }

        public DefaultKey() {
            InitializeComponent();
            DataContext = this;
        }

        private void onMouseEnter(object sender, MouseEventArgs e) {
            shadow.Opacity = 0.3;
        }

        private void onMouseleave(object sender, MouseEventArgs e) {
            shadow.Opacity = 0;
        }
    }
}
