using System;
using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Database.Data.Diskpart;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter;


namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaktionslogik für CreatePartition.xaml
    /// </summary>
    public partial class CreatePartitionWindow : Window
    {
        PMainWindow<GUIFDMainWin> MainWindow = App.Instance.WIM.GetPresenter<PMainWindow<GUIFDMainWin>>();

        long sizeInMB;

        private DiskModel diskModel;
        public DiskModel DiskModel
        {
            get { return diskModel; }
            set
            {
                diskModel = value;
                AddTextToConsole();
            }
        }

        public CreatePartitionWindow(DiskModel disk)
        {
            InitializeComponent();

            diskModel = disk;
        }

        public CreatePartitionWindow(DiskModel disk, long size)
        {
            InitializeComponent();

            size /= 1024;
            size /= 1024;
            this.sizeInMB = size;
            diskModel = disk;
            SizeValue.Text = size.ToString();
        }

        private string SelectedOptionAsString()
        {
            return (string)((ComboBoxItem)PartitionOptionValue.SelectedValue).Content;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string option = DPCreatePartition.EFI;

            switch (SelectedOptionAsString())
            {
                case ("EFI"):
                    option = DPCreatePartition.EFI;
                    break;
                case ("EXTENDED"):
                    option = DPCreatePartition.EXTENDED;
                    break;
                case ("LOGICAL"):
                    option = DPCreatePartition.LOGICAL;
                    break;
                case ("MSR"):
                    option = DPCreatePartition.MSR;
                    break;
                case ("PRIMARY"):
                    option = DPCreatePartition.PRIMARY;
                    break;
            }


            string output = string.Empty;
            
            output += DPFunctions.CreatePartition(DiskModel.DiskIndex, option, GetSizeValue(), false);

            MainWindow.Log.Print(output);
            MainWindow.DisplayDiskData(false);

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
            ConsoleReturn.Print(DiskModel.GetOutputAsString());
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
