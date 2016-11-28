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
    /// Interaction logic for TabKey.xaml
    /// </summary>
    public partial class TabKey : UserControl {

        public static readonly DependencyProperty TopLegendProperty = DependencyProperty.Register("TabTopLegend", typeof(string), typeof(TabKey));
        public static readonly DependencyProperty BotLegendProperty = DependencyProperty.Register("TabBotLegend", typeof(string), typeof(TabKey));

        public string TabTopLegend {
            get { return (string)GetValue(TopLegendProperty); }
            set { SetValue(TopLegendProperty, value); }
        }

        public string TabBotLegend {
            get { return (string)GetValue(BotLegendProperty); }
            set { SetValue(BotLegendProperty, value); }
        }

        public TabKey() {
            InitializeComponent();
            DataContext = this;
        }
    }
}
