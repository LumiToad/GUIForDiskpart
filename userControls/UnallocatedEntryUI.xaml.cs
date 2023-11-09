using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUIForDiskpart.userControls
{
    /// <summary>
    /// Interaction logic for UnallocatedEntryUI.xaml
    /// </summary>
    public partial class UnallocatedEntryUI : UserControl
    {
        MainWindow MainWindow => (MainWindow)Application.Current.MainWindow;
        public Dictionary<string, object?> Entry
        {
            get 
            {
                Dictionary<string, object?> dict = new Dictionary<string, object?>();
                dict.Add("Unallocated Space", Size.Content); 

                return dict;
            }
        }

        public bool? IsSelected { get { return EntrySelected.IsChecked; } }

        public UnallocatedEntryUI(string size)
        {
            InitializeComponent();
            SetSize(size);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectEntryRadioButton();
            MainWindow.UnallocatedEntry_Click(this);
        }

        public void SelectEntryRadioButton()
        {
            EntrySelected.IsChecked = !EntrySelected.IsChecked;
        }

        private void OpenContextMenu_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu.IsOpen = !ContextMenu.IsOpen;
        }

        private void SetSize(string size)
        {
            Size.Content = size;
        }
    }
}
