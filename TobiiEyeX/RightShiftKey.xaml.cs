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
    /// Interaction logic for RightShiftKey.xaml
    /// </summary>
    public partial class RightShiftKey : AbstractKey {

        public RightShiftKey() {
            InitializeComponent();
        }

        public override void toggle() {
            if (toggled) {
                shadow.Opacity = 0;
                toggled = false;
            }
            else {
                shadow.Opacity = (double)Application.Current.Resources["HighlightOpacity"];
                toggled = true;
            }
        }

        public override void progressHighlight(double ratio) {
            highlight.Height = ActualHeight * ratio;
        }

        public override void resetHighlight() {
            highlight.Height = 0;
        }

        private void onMouseEnter(object sender, MouseEventArgs e) {
            shadow.Opacity = (double)Application.Current.Resources["HighlightOpacity"];
        }

        private void onMouseleave(object sender, MouseEventArgs e) {
            if (!toggled) shadow.Opacity = 0;
        }
    }
}
