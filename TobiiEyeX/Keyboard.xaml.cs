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
using EyeXFramework.Wpf;

namespace TobiiEyeX {
    /// <summary>
    /// Interaction logic for Keyboard.xaml
    /// </summary>
    public partial class Keyboard : Window {

        private int lastHit;
        private bool isShiftPressed = false;
        private bool isCapsLockPressed = false;
        private Dictionary<Type, int> keyTypes = new Dictionary<Type, int> {
            { typeof(DefaultKey), 0 },
            { typeof(SpaceKey), 1 },
            { typeof(EnterKey), 2 },
            { typeof(LeftShiftKey), 3 },
            { typeof(RightShiftKey), 4 },
            { typeof(ControlKey), 5 },
            { typeof(TabKey), 6 },
            { typeof(CapslockKey), 7 },
            { typeof(BackspaceKey), 8 }
        };

        public Keyboard() {
            InitializeComponent();
            inputBox.Focus();
        }

        private void onHasGazeChanged(object sender, RoutedEventArgs e) {
            bool wasEdited = true;
            switch (keyTypes[e.Source.GetType()]) {
                case 0:
                    // Default
                    DefaultKey commonKey = e.Source as DefaultKey;
                    if (!e.Source.GetHashCode().Equals(lastHit)) {
                        if (commonKey.IsAlphabet) {
                            if (isShiftPressed || isCapsLockPressed) inputBox.Text += commonKey.TopLegend.ToUpper();
                            else inputBox.Text += commonKey.TopLegend.ToLower();
                        }
                        else {
                            if (isShiftPressed) inputBox.Text += commonKey.TopLegend;
                            else inputBox.Text += commonKey.BotLegend;
                        }
                    }
                    commonKey.toggle();
                    break;
                case 1:
                    // Space
                    SpaceKey spaceKey = e.Source as SpaceKey;
                    if (!e.Source.GetHashCode().Equals(lastHit)) {
                        inputBox.Text += " ";
                    }
                    spaceKey.toggle();
                    break;
                case 2:
                    // Enter
                    EnterKey enterKey = e.Source as EnterKey;
                    if (!e.Source.GetHashCode().Equals(lastHit)) {
                        inputBox.Text += Environment.NewLine;
                    }
                    enterKey.toggle();
                    break;
                case 3:
                case 4:
                    // Shifts
                    if (!e.Source.GetHashCode().Equals(lastHit)) {
                        leftShift.toggle();
                        rightShift.toggle();
                        isShiftPressed = !isShiftPressed;
                    }
                    wasEdited = false;
                    break;
                case 5:
                    // Ctrl
                    ControlKey controlKey = e.Source as ControlKey;
                    System.Diagnostics.Debug.WriteLine(controlKey.TopLegend + " was pressed.");
                    controlKey.toggle();
                    wasEdited = false;
                    break;
                case 6:
                    // Tab
                    TabKey tabKey = e.Source as TabKey;
                    if (!e.Source.GetHashCode().Equals(lastHit)) {
                        if (tabKey.IsActualTab) inputBox.Text += "\t";
                        else {
                            if (isShiftPressed) inputBox.Text += tabKey.BotLegend;
                            else inputBox.Text += tabKey.TopLegend;
                        }
                    }
                    tabKey.toggle();
                    break;
                case 7:
                    // Caps
                    if (!e.Source.GetHashCode().Equals(lastHit)) {
                        capsLock.toggle();
                        isCapsLockPressed = !isCapsLockPressed;
                    }
                    wasEdited = false;
                    break;
                case 8:
                    // Backspace
                    BackspaceKey backspaceKey = e.Source as BackspaceKey;
                    if (!e.Source.GetHashCode().Equals(lastHit)) {
                        if (inputBox.Text.Length > 0) {
                            inputBox.Text = inputBox.Text.Substring(0, inputBox.Text.Length - 1);
                        }
                    }
                    backspaceKey.toggle();
                    break;
                default:
                    // Unknown
                    break;
            }
            if (wasEdited) {
                inputBox.Focus();
                inputBox.Select(inputBox.Text.Length, 0);
            }
            lastHit = e.Source.GetHashCode();
        }

        private void onMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            // System.Diagnostics.Debug.WriteLine
            bool wasEdited = true;
            switch (keyTypes[e.Source.GetType()]) {
                case 0:
                    // Default
                    DefaultKey commonKey = e.Source as DefaultKey;
                    if (commonKey.IsAlphabet) {
                        if (isShiftPressed || isCapsLockPressed) inputBox.Text += commonKey.TopLegend.ToUpper();
                        else inputBox.Text += commonKey.TopLegend.ToLower();
                    }
                    else {
                        if (isShiftPressed) inputBox.Text += commonKey.TopLegend;
                        else inputBox.Text += commonKey.BotLegend;
                    }
                    break;
                case 1:
                    // Space
                    SpaceKey spaceKey = e.Source as SpaceKey;
                    inputBox.Text += " ";
                    break;
                case 2:
                    // Enter
                    EnterKey enterKey = e.Source as EnterKey;
                    inputBox.Text += Environment.NewLine;
                    break;
                case 3:
                case 4:
                    // Shifts
                    leftShift.toggle();
                    rightShift.toggle();
                    isShiftPressed = !isShiftPressed;
                    wasEdited = false;
                    break;
                case 5:
                    // Ctrl
                    ControlKey controlKey = e.Source as ControlKey;
                    System.Diagnostics.Debug.WriteLine(controlKey.TopLegend + " was pressed.");
                    wasEdited = false;
                    break;
                case 6:
                    // Tab
                    TabKey tabKey = e.Source as TabKey;
                    if (tabKey.IsActualTab) inputBox.Text += "\t";
                    else {
                        if (isShiftPressed) inputBox.Text += tabKey.BotLegend;
                        else inputBox.Text += tabKey.TopLegend;
                    }
                    break;
                case 7:
                    // Caps
                    capsLock.toggle();
                    isCapsLockPressed = !isCapsLockPressed;
                    wasEdited = false;
                    break;
                case 8:
                    // Backspace
                    BackspaceKey backspaceKey = e.Source as BackspaceKey;
                    if (inputBox.Text.Length > 0) {
                        inputBox.Text = inputBox.Text.Substring(0, inputBox.Text.Length - 1);
                    }
                    break;
                default:
                    // Unknown
                    break;
            }
            if (wasEdited) {
                inputBox.Focus();
                inputBox.Select(inputBox.Text.Length, 0);
            }
        }
    }
}
