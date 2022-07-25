using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SeewoLamp
{
    /// <summary>
    /// Timeline.xaml 的交互逻辑
    /// </summary>
    public partial class Timeline : Window
    {
        [DllImport("user32.dll"/*, EntryPoint = "SetWindowCompositionAttribute"*/)]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
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
        public Timeline()
        {
            InitializeComponent();
            //this.Opacity = 0.1;

            string str2 = "2023-6-7 0:00:01";
            DateTime d1 = DateTime.Now;
            DateTime d2 = Convert.ToDateTime(str2);
            DateTime d3 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", d1.Year, d1.Month, d1.Day));
            DateTime d4 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", d2.Year, d2.Month, d2.Day));
            string days = Convert.ToString((d4 - d3).Days);
            this.text.Content = "距离高考还有" + days + "天";

            List<string> slogansKey = new List<string>();
            Random random = new Random();
            int randint = random.Next(0, 299);
            string workDirectory = Directory.GetCurrentDirectory();
            string thePath = $@"{workDirectory}\slogans\thepoems.txt";
            StreamReader ssr = new StreamReader(thePath, Encoding.Default);
            String line;
            while ((line = ssr.ReadLine()) != null)
            {
                slogansKey.Add(line);
            }
            SloganTexts.Text = slogansKey[randint];

            var thread = new Thread(() =>
            {
                _lessons = new Lessons();
                _lessons.Closed += (sender, args) =>
                {
                    // when window is closed - shutdown dispatcher
                    _lessons.Dispatcher.InvokeShutdown();
                };
                _lessons.Show();
                // run dispatcher (message pump) on this thread
                System.Windows.Threading.Dispatcher.Run();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();



        }


        private Lessons _lessons;
        private void Grid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //this.DragMove();
        }

        private void Grid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //this.Close();
        }

        private void Grid_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //this.Close();
            this.Dispatcher.Invoke(() => this.Close());
        }

        private void ChangeIt_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                List<string> slogansKey = new List<string>();
                Random random = new Random();
                int randint = random.Next(0, 299);
                string workDirectory = Directory.GetCurrentDirectory();
                string thePath = $@"{workDirectory}\slogans\thepoems.txt";
                StreamReader ssr = new StreamReader(thePath, Encoding.Default);
                String line;
                while ((line = ssr.ReadLine()) != null)
                {
                    slogansKey.Add(line);
                }
                SloganTexts.Text = slogansKey[randint];
            });
        }


        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //EnableBlur();
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            EnableBlur();
        }
    }
}
