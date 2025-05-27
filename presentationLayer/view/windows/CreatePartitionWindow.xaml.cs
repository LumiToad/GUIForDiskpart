using GUIForDiskpart.Database.Data.Diskpart;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.ModelLayer;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaktionslogik für CreatePartition.xaml
    /// </summary>
    public partial class CreatePartitionWindow : Window
    {
        MainWindow MainWindow => (MainWindow)Application.Current.MainWindow;

        long sizeInMB;

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

        public CreatePartitionWindow(DiskInfo disk)
        {
            InitializeComponent();

            DiskInfo = disk;
        }

        public CreatePartitionWindow(DiskInfo disk, long size)
        {
            InitializeComponent();

            size /= 1024;
            size /= 1024;
            this.sizeInMB = size;
            DiskInfo = disk;
            SizeValue.Text = size.ToString();
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

        private void SizeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SizeValue.Text.Length == 0) return;

            long enteredSize = Convert.ToInt64(SizeValue.Text);
            if (enteredSize > sizeInMB)
            {
                SizeValue.Text = sizeInMB.ToString();
            }
        }
    }
}
