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
            { typeof(BackspaceKey), 8 },
            { typeof(ArrowKey), 9 }
        };

        private Timer timer;

        public Keyboard() {
            InitializeComponent();
            eyeXHost = new WpfEyeXHost();
            eyeXHost.Start();
            inputBox.Focus();
        }

        private void onHasGazeChanged(object sender, RoutedEventArgs e) {
            object source = e.Source;
            AbstractKey aKey = source as AbstractKey;
            if (timer != null && timer.isRunning) {
                // If first initialization or timer is already running
                // Stop it and refresh key, because we moving to a new one
                // Stopping timer and reseting highlight
                timer.Stop();
                aKey.resetHighlight();
            }
            else {
                // If timer is not running 
                // Just run it
                timer = new Timer((int)Application.Current.Resources["Threshold"], 100, true);
                timer.OnElapsed += delegate () {
                    captureKey(source);
                    aKey.resetHighlight();
                };
                timer.OnTick += delegate (double ratio) {
                    aKey.progressHighlight(ratio);
                };
                timer.Start();
            }
            // And anyway toggle key
            toggleKey(source);
        }

        private void captureKey(object source) {
            bool wasEdited = true;
            int selection = inputBox.SelectionStart;
            switch (keyTypes[source.GetType()]) {
                case 0:
                    // Default
                    DefaultKey commonKey = source as DefaultKey;
                    if (commonKey.IsAlphabet) {
                        if (isShiftPressed || isCapsLockPressed) inputBox.Text = inputBox.Text.Insert(inputBox.SelectionStart, commonKey.TopLegend.ToUpper());
                        else inputBox.Text = inputBox.Text.Insert(inputBox.SelectionStart, commonKey.TopLegend.ToLower());
                    }
                    else {
                        if (isShiftPressed) inputBox.Text = inputBox.Text.Insert(inputBox.SelectionStart, commonKey.TopLegend);
                        else inputBox.Text = inputBox.Text.Insert(inputBox.SelectionStart, commonKey.BotLegend);
                    }
                    selection++;
                    break;
                case 1:
                    // Space
                    SpaceKey spaceKey = source as SpaceKey;
                    inputBox.Text = inputBox.Text.Insert(inputBox.SelectionStart, " ");
                    selection++;
                    break;
                case 2:
                    // Enter
                    EnterKey enterKey = source as EnterKey;
                    inputBox.Text = inputBox.Text.Insert(inputBox.SelectionStart, Environment.NewLine);
                    selection += 2;
                    break;
                case 3:
                case 4:
                    // Shifts
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
                    if (tabKey.IsActualTab) inputBox.Text = inputBox.Text.Insert(inputBox.SelectionStart, "\t");
                    else {
                        if (isShiftPressed) inputBox.Text = inputBox.Text.Insert(inputBox.SelectionStart, tabKey.BotLegend);
                        else inputBox.Text = inputBox.Text.Insert(inputBox.SelectionStart, tabKey.TopLegend);
                    }
                    selection++;
                    break;
                case 7:
                    // Caps
                    isCapsLockPressed = !isCapsLockPressed;
                    wasEdited = false;
                    break;
                case 8:
                    // Backspace
                    BackspaceKey backspaceKey = source as BackspaceKey;
                    if (inputBox.Text.Length > 0 && inputBox.SelectionStart > 0) {
                        inputBox.Text = inputBox.Text.Remove(inputBox.SelectionStart - 1, 1);
                        selection--;
                    }
                    break;
                case 9:
                    ArrowKey arrowKey = source as ArrowKey;
                    moveSelection(arrowKey.Direction);
                    wasEdited = false;
                    break;
                default:
                    // Unknown
                    break;
            }
            if (wasEdited) {
                inputBox.Focus();
                inputBox.Select(selection, 0);
            }
        }

        // Only use with eye tracking!!!
        private void toggleKey(object source) {
            // Basically we switch state for general keys
            // And for shift and caps only if they are not already toggled
            switch (keyTypes[source.GetType()]) {
                case 3:
                case 4:
                    // Shifts
                    if (!isShiftPressed) leftShift.toggle();
                    break;
                case 7:
                    // Caps
                    if (!isCapsLockPressed) capsLock.toggle();
                    break;
                default:
                    // All others
                    AbstractKey key = source as AbstractKey;
                    key.toggle();
                    break;
            }
        }

        private void moveSelection(DirectionType direction) {
            switch (direction) {
                case DirectionType.UP: {
                        int currentLine = inputBox.GetLineIndexFromCharacterIndex(inputBox.SelectionStart);
                        if (currentLine == 0) return;
                        int shift = inputBox.SelectionStart - inputBox.GetCharacterIndexFromLineIndex(currentLine);
                        int previousLineLength = inputBox.GetLineLength(currentLine - 1);
                        int previousLineStart = inputBox.GetCharacterIndexFromLineIndex(currentLine - 1);
                        int destination = shift + previousLineStart;
                        if (shift <= (previousLineLength-2) && destination < inputBox.Text.Length) inputBox.Select(destination, 0);
                        else inputBox.Select(previousLineStart + previousLineLength - 2, 0);
                    }
                    break;
                case DirectionType.DOWN: {
                        int currentLine = inputBox.GetLineIndexFromCharacterIndex(inputBox.SelectionStart);
                        if (currentLine == (inputBox.LineCount - 1)) return;
                        int shift = inputBox.SelectionStart - inputBox.GetCharacterIndexFromLineIndex(currentLine);
                        int nextLineLength = inputBox.GetLineLength(currentLine + 1);
                        int nextLineStart = inputBox.GetCharacterIndexFromLineIndex(currentLine + 1);
                        int destination = shift + nextLineStart;
                        if (shift <= nextLineLength && destination < inputBox.Text.Length) inputBox.Select(destination, 0);
                        else inputBox.Select(nextLineStart + nextLineLength, 0);
                    }
                    break;
                case DirectionType.LEFT:
                    if (inputBox.SelectionStart > 0) {
                        inputBox.Select(inputBox.SelectionStart - 1, 0);
                    }
                    break;
                case DirectionType.RIGHT:
                    if (inputBox.SelectionStart < inputBox.Text.Length) {
                        inputBox.Select(inputBox.SelectionStart + 1, 0);
                    }
                    break;
                case DirectionType.UNDEFINED:
                    break;
                default:
                    break;
            }
        }

        private void onMouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            if (timer != null) timer.Stop();
            AbstractKey aKey = e.Source as AbstractKey;
            aKey.resetHighlight();
        }

        private void onMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            timer = new Timer((int)Application.Current.Resources["Threshold"], 10, true);
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
