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
using MyExcel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using System.Reflection;

namespace SeewoLamp
{

    /// <summary>
    /// Lessons.xaml 的交互逻辑
    /// </summary>
    public partial class Lessons : Window
    {
        [DllImport("user32.dll"/*, EntryPoint = "SetWindowCompositionAttribute"*/)]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
        public Lessons()
        {
            InitializeComponent();
            CycleText.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
            string workDirectory = Directory.GetCurrentDirectory();
            //string path = $@"{workDirectory}\Monday.txt";
            //StreamReader sr = new StreamReader(path, Encoding.Default);
            //String line;
            var lessonsKey = new List<string>();
            if (CycleText.Text == "星期日")
            {
                string path = $@"{workDirectory}\Monday.txt";
                StreamReader sr = new StreamReader(path, Encoding.Default);
                String line;
                while ((line = sr.ReadLine()) != null)
                {

                    lessonsKey.Add(line);
                }
                One.Text = lessonsKey[0];
                Two.Text = lessonsKey[1];
                Three.Text = lessonsKey[2];
                Four.Text = lessonsKey[3];
                Five.Text = lessonsKey[4];
                Six.Text = lessonsKey[5];
                Seven.Text = lessonsKey[6];
                Eight.Text = lessonsKey[7];
                Nine.Text = lessonsKey[8];
            }
            

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

        private void LessonsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            EnableBlur();
        }


        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Grid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
