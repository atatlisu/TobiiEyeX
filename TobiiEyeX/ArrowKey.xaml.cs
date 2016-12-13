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
    /// Interaction logic for ArrowKey.xaml
    /// </summary>
    /// 

    public enum DirectionType {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        UNDEFINED
    }

    public partial class ArrowKey : AbstractKey {

        public static readonly DependencyProperty ArrowProperty = DependencyProperty.Register("Arrow", typeof(string), typeof(ArrowKey));
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(DirectionType), typeof(ArrowKey));

        public string Arrow {
            get { return (string)GetValue(ArrowProperty); }
            set { SetValue(ArrowProperty, value); }
        }

        public DirectionType Direction {
            get { return (DirectionType)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public ArrowKey() {
            InitializeComponent();
            DataContext = this;
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
            shadow.Opacity = 0;
        }
    }
}
