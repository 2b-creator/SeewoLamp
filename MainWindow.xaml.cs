using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Threading;
using System.Windows.Threading;

namespace SeewoLamp
{
    public delegate void dlgtOk();
    internal enum AccentState
    {
        ACCENT_DISABLED = 1,
        ACCENT_ENABLE_GRADIENT = 0,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_ENABLE_BLURBEHIND = 3,
        ACCENT_INVALID_STATE = 4
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public int AccentFlags;
        public int GradientColor;
        public int AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    internal enum WindowCompositionAttribute
    {
        // ...
        WCA_ACCENT_POLICY = 19
        // ...
    }
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll"/*, EntryPoint = "SetWindowCompositionAttribute"*/)]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
        //private static readonly string TipToolAll = "F:\\Csharp\\SeewoLamp\\op.png";
        public MainWindow()
        {
            InitializeComponent();
            //this.Background = new ImageBrush
            //{
            //    ImageSource = new BitmapImage(new Uri(TipToolAll))
            //};


        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
            EnableBlur();
            //this.Left = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 300;
            //this.Top = 300;
        }
        internal void EnableBlur()
        {
            var windowHelper = new WindowInteropHelper(this);

            var accent = new AccentPolicy();
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

            var accentStructSize = Marshal.SizeOf(accent);

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;

            SetWindowCompositionAttribute(windowHelper.Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //this.DragMove();
        }
        private Timeline _timeline;
        private void Timetable_Click(object sender, RoutedEventArgs e)
        {
            var thread = new Thread(() =>
            {
                _timeline = new Timeline();
                _timeline.Closed += (sender, args) =>
                {
                    // when window is closed - shutdown dispatcher
                    _timeline.Dispatcher.InvokeShutdown();
                };
                _timeline.Show();
                // run dispatcher (message pump) on this thread
                System.Windows.Threading.Dispatcher.Run();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            //Dispatcher.BeginInvoke(new Action(async delegate
            //{

            //Timeline timeline = new Timeline();
            //dlgtOk dlgtok = new dlgtOk(ShowTheDialogPlus);
            //this.Dispatcher.BeginInvoke(dlgtok);

            //Action<Timeline> action = new Action<Timeline>(ShowTheDialogue);
            //await timeline.Dispatcher.BeginInvoke(action);
            //timeline.ShowDialog();
            //}));
        }
        static void StartLessionsTable()
        {
            //启用Lessons
            //Dispatcher.BeginInvoke(new Action(delegate
            //{
            //    Lessons lessons = new Lessons();
            //    lessons.ShowDialog();
            //}));

        }

        private void Grid_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            _timeline.Dispatcher.Invoke(() => _timeline.Close());
        }

        //private async Task<string> ShowTheDialogue()
        //{
        //    return await Task.Run(() =>
        //    {
        //        Timeline timeline = new Timeline();
        //        timeline.ShowDialog();
        //        return "0";
        //    });
        //}
        public void ShowTheDialogue(Timeline timeline)
        {
            timeline.WindowStartupLocation = WindowStartupLocation.Manual;
            int xtimelineWidth = SystemInformation.PrimaryMonitorSize.Width;
            int yinttimelineHeight = SystemInformation.PrimaryMonitorSize.Height;
            Double ytimelineHeight = Convert.ToDouble(yinttimelineHeight);
            timeline.Left = xtimelineWidth / 4;
            timeline.Top = ytimelineHeight / 1.8;
            timeline.ShowDialog();
        }
        private void ShowTheDialogPlus()
        {
            Timeline timeline = new Timeline();
            timeline.WindowStartupLocation = WindowStartupLocation.Manual;
            int xtimelineWidth = SystemInformation.PrimaryMonitorSize.Width;
            int yinttimelineHeight = SystemInformation.PrimaryMonitorSize.Height;
            Double ytimelineHeight = Convert.ToDouble(yinttimelineHeight);
            timeline.Left = xtimelineWidth / 4;
            timeline.Top = ytimelineHeight / 1.8;
            timeline.ShowDialog();
        }
    }
}
