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
using Tobii.EyeX.Framework;
using TobiiEyeX.Utils;

namespace TobiiEyeX {
    /// <summary>
    /// Interaction logic for Keyboard.xaml
    /// </summary>
    public partial class Keyboard : Window {

        private WpfEyeXHost eyeXHost;
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

        private Timer timer;

        public Keyboard() {
            InitializeComponent();
            eyeXHost = new WpfEyeXHost();
            eyeXHost.Start();
            inputBox.Focus();
        }

        private void onHasGazeChanged(object sender, RoutedEventArgs e) {
            // Stopping timer and reseting highlight
            if (timer != null) timer.Stop();
            AbstractKey aKey = e.Source as AbstractKey;
            aKey.resetHighlight();

            // Launching timer
            timer = new Timer((int)Application.Current.Resources["Threshold"], 100, true);
            object source = e.Source;
            toggleKey(source);
            timer.OnElapsed += delegate () {
                captureKey(source);
                aKey.resetHighlight();
            };
            timer.OnTick += delegate (double ratio) {
                aKey.progressHighlight(ratio);
            };
            timer.Start();
        }

        private void captureKey(object source) {
            bool wasEdited = true;
            switch (keyTypes[source.GetType()]) {
                case 0:
                    // Default
                    DefaultKey commonKey = source as DefaultKey;
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
                    SpaceKey spaceKey = source as SpaceKey;
                    inputBox.Text += " ";
                    break;
                case 2:
                    // Enter
                    EnterKey enterKey = source as EnterKey;
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
                    ControlKey controlKey = source as ControlKey;
                    System.Diagnostics.Debug.WriteLine(controlKey.TopLegend + " was pressed.");
                    wasEdited = false;
                    break;
                case 6:
                    // Tab
                    TabKey tabKey = source as TabKey;
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
                    BackspaceKey backspaceKey = source as BackspaceKey;
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

        // Only use with eye tracking!!!
        private void toggleKey(object source) {
            // Basically we switch state for general keys
            // And for shifts and capses only if they are not already toggled
            switch (keyTypes[source.GetType()]) {
                case 3:
                case 4:
                    // Shifts
                    if (!leftShift.toggled && !rightShift.toggled) {
                        leftShift.toggle();
                        rightShift.toggle();
                    }
                    break;
                case 7:
                    // Caps
                    if (!capsLock.toggled) capsLock.toggle();
                    break;
                default:
                    // All others
                    AbstractKey key = source as AbstractKey;
                    key.toggle();
                    break;
            }
        }

        private void onMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            if (timer != null) timer.Stop();
            AbstractKey aKey = e.Source as AbstractKey;
            aKey.resetHighlight();
        }

        private void onMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            timer = new Timer((int)Application.Current.Resources["Threshold"], 100, true);
            object source = e.Source;
            AbstractKey aKey = source as AbstractKey;
            timer.OnElapsed += delegate () {
                captureKey(source);
                aKey.resetHighlight();
            };
            timer.OnTick += delegate (double ratio) {
                aKey.progressHighlight(ratio);
            };
            timer.Start();
        }

        private void onClose(object sender, EventArgs e) {
            eyeXHost.Dispose();
        }
    }
}
