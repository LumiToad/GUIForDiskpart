using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GUIForDiskpart.Presentation.Presenter;
using GUIForDiskpart.Utils;

namespace GUIForDiskpart.Presentation.View.UserControls
{
    public partial class EntryDataUI : UserControl
    {
        public EntryDataUI()
        {
            InitializeComponent();
        }

        DataGridCellInfo? selectedCell;

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            EntryDataGrid.SelectAllCells();

            List<DataGridCellInfo> cells = new List<DataGridCellInfo>();

            foreach (var entry in EntryDataGrid.SelectedCells)
            {
                if (entry.Item.ToString().Contains(SearchBar.Text, System.StringComparison.OrdinalIgnoreCase))
                {
                    cells.Add(entry);
                }
            }

            EntryDataGrid.UnselectAllCells();

            if (cells.Count > 0)
            {
                foreach (var cell in cells)
                {
                    EntryDataGrid.SelectedCells.Add(cell);
                }
            }

            if (string.IsNullOrEmpty(SearchBar.Text))
            {
                EntryDataGrid.UnselectAllCells();
            }
        }

        private void SearchBar_CursorFocus(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!SearchBar.IsFocused)
            {
                SearchBar.Text = "";
            }
        }

        public void SaveEntryData_Click(object sender, RoutedEventArgs e)
        {
            string entrieString = string.Empty;

            bool noneSelected = false;

            if (EntryDataGrid.SelectedCells.Count == 0)
            {
                EntryDataGrid.SelectAllCells();
                noneSelected = true;
            }

            foreach (var entry in EntryDataGrid.SelectedCells)
            {
                if (entry.Item == null) continue;
                if (entrieString.Contains(entry.Item.ToString())) continue;
                entrieString += entry.Item + "\n";
            }

            if (noneSelected)
            {
                EntryDataGrid.UnselectAllCells();
            }

            FileUtils.SaveAsTextfile(entrieString, "data");
        }

        public void AddDataToGrid(Dictionary<string, object?> data)
        {
            EntryDataGrid.ItemsSource = data;
        }

        private void EntryDataGrid_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            if (EntryDataGrid.SelectedCells.Count == 1)
            {
                if (selectedCell == EntryDataGrid.SelectedCells[0])
                {
                    EntryDataGrid.UnselectAllCells();
                    selectedCell = null;
                    return;
                }
                selectedCell = EntryDataGrid.SelectedCells[0];
            }
        }
    }
}
