using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GUIForDiskpart.Presentation.Presenter;
using GUIForDiskpart.Utils;

namespace GUIForDiskpart.Presentation.View.UserControls
{
    public partial class UCEntryData : UserControl
    {
        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick ESaveEntryData;

        public delegate void DOnLMBUp(object sender, MouseButtonEventArgs e);
        public event DOnLMBUp ESelection;

        public delegate void DOnTextChanged(object sender, TextChangedEventArgs e);
        public event DOnTextChanged ESearchBarText;

        public delegate void DOnCursorFocus(object sender, DependencyPropertyChangedEventArgs e);
        public event DOnCursorFocus ESearchBarFocused;

        public UCEntryData()
        {
            InitializeComponent();
        }

        public DataGridCellInfo? SelectedCell {  get; set; }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e) => ESearchBarText?.Invoke(sender, e);
        private void SearchBar_CursorFocus(object sender, DependencyPropertyChangedEventArgs e) => ESearchBarFocused?.Invoke(sender, e);
        public void SaveEntryData_Click(object sender, RoutedEventArgs e) => ESaveEntryData?.Invoke(sender, e);
        private void EntryDataGrid_LMBUp(object sender, MouseButtonEventArgs e) => ESelection?.Invoke(sender, e);
    }
}
