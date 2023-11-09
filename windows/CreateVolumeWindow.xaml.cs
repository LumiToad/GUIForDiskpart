using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
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

namespace GUIForDiskpart.windows
{
    /// <summary>
    /// Interaction logic for CreateVolumeWindow.xaml
    /// </summary>
    public partial class CreateVolumeWindow : Window
    {
        MainWindow MainWindow => (MainWindow)Application.Current.MainWindow;

        private DiskInfo diskInfo;
        public DiskInfo DiskInfo
        {
            get { return diskInfo; }
            set
            {
                diskInfo = value;
                AddTextToConsole();
            }
        }

        public CreateVolumeWindow(DiskInfo disk)
        {
            InitializeComponent();
            DiskInfo = disk;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            output += DPFunctions.CreateVolume(diskInfo.DiskIndex, CreateVolumeOptions.SIMPLE, GetSizeValue(), false);

            MainWindow.AddTextToOutputConsole(output);
            MainWindow.RetrieveAndShowDiskData(false);

            this.Close();
        }

        private UInt64 GetSizeValue()
        {
            UInt64 size = 0;

            if (SizeValue.Text != "")
            {
                UInt64.TryParse(SizeValue.Text, out size);
            }

            return size;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void AddTextToConsole()
        {
            ConsoleReturn.AddTextToOutputConsole(DiskInfo.GetOutputAsString());
        }

        
    }
}
