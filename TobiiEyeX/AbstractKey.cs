using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TobiiEyeX {

    public partial class AbstractKey : UserControl {

        public bool toggled = false;

        public AbstractKey() {
            
        }

        public virtual void toggle() {

        }

        public virtual void progressHighlight(double ratio) {

        }

        public virtual void resetHighlight() {

        }
    }
}
