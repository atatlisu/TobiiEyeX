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
    /// Interaction logic for EnterKey.xaml
    /// </summary>
    public partial class EnterKey : UserControl {

        private bool toggled = false;

        public EnterKey() {
            InitializeComponent();
        }

        public void toggle() {
            if (toggled) {
                shadow.Opacity = 0;
                toggled = false;
            }
            else {
                shadow.Opacity = (double)Application.Current.Resources["HighlightOpacity"];
                toggled = true;
            }
        }

        private void onMouseEnter(object sender, MouseEventArgs e) {
            shadow.Opacity = (double)Application.Current.Resources["HighlightOpacity"];
        }

        private void onMouseleave(object sender, MouseEventArgs e) {
            shadow.Opacity = 0;
        }
    }
}
