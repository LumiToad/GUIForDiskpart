using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GUIForDiskpart.windows
{
    /// <summary>
    /// Interaktionslogik für CreatePartition.xaml
    /// </summary>
    public partial class CreatePartitionWindow : Window
    {
        private Int64 freeSpace;

        private MainWindow mainWindow;

        private DiskInfo diskInfo;

        public CreatePartitionWindow(DiskInfo disk, long freeSpace)
        {
            InitializeComponent();


            mainWindow = (MainWindow)Application.Current.MainWindow;
            this.freeSpace = freeSpace;
            this.diskInfo = disk;
        }

        private string SelectedOptionAsString()
        {
            return (string)((ComboBoxItem)PartitionOptionValue.SelectedValue).Content;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string option = CreatePartitionOptions.EFI;

            switch (SelectedOptionAsString())
            {
                case ("EFI"):
                    option = CreatePartitionOptions.EFI;
                    break;
                case ("EXTENDED"):
                    option = CreatePartitionOptions.EXTENDED;
                    break;
                case ("LOGICAL"):
                    option = CreatePartitionOptions.LOGICAL;
                    break;
                case ("MSR"):
                    option = CreatePartitionOptions.MSR;
                    break;
                case ("PRIMARY"):
                    option = CreatePartitionOptions.PRIMARY;
                    break;
            }


            string output = string.Empty;
            
            output += DPFunctions.CreatePartition(diskInfo.DiskIndex, option, GetSizeValue(), false);

            mainWindow.AddTextToOutputConsole(output);
            mainWindow.RetrieveAndShowDiskData(false);

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

        public void DriveInfoToTextBox()
        {
            DiskDetailValue.Text = diskInfo.GetOutputAsString();
        }
    }
}
