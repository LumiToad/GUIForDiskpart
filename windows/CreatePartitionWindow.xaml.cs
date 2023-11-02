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
        private UInt64 freeSpace;

        private MainWindow mainWindow;

        private DiskInfo driveInfo;

        public CreatePartitionWindow(DiskInfo drive, ulong freeSpace)
        {
            InitializeComponent();


            mainWindow = (MainWindow)Application.Current.MainWindow;
            this.freeSpace = freeSpace;
            this.driveInfo = drive;
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
            
            output += DPFunctions.CreatePartition(driveInfo.DiskIndex, option, GetSizeValue(), false);

            mainWindow.AddTextToOutputConsole(output);
            mainWindow.RetrieveAndShowDriveData(false);

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
            DiskDetailValue.Text = driveInfo.GetOutputAsString();
        }
    }
}
