using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GUIForDiskpart.main;

namespace GUIForDiskpart.userControls
{
    public partial class EntryData : UserControl
    {
        public EntryData()
        {
            InitializeComponent();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            EntryDataGrid.SelectAllCells();

            List<DataGridCellInfo> cells = new List<DataGridCellInfo>();

            foreach (var entry in EntryDataGrid.SelectedCells)
            {
                if (entry.Item.ToString().Contains(SearchBar.Text))
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

        private void SaveEntryData_Click(object sender, RoutedEventArgs e)
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
                entrieString += entry.Item + "\n";
            }

            if (noneSelected)
            {
                EntryDataGrid.UnselectAllCells();
            }

            SaveFile.SaveAsTextfile(entrieString, "data");
        }
    }
}
