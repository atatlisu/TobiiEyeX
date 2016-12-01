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
using System.Windows.Shapes;

namespace TobiiEyeX {
    /// <summary>
    /// Interaction logic for Keyboard.xaml
    /// </summary>
    public partial class Keyboard : Window {
        public Keyboard() {
            InitializeComponent();
        }

        private void onMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            if (e.Source is DefaultKey) {
                DefaultKey key = e.Source as DefaultKey;
                System.Diagnostics.Debug.WriteLine("Clicked on " + key.TopLegend);
            }
        }
    }
}
