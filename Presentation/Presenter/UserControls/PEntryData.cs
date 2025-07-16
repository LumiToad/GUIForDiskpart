global using PEntryData =
    GUIForDiskpart.Presentation.Presenter.UserControls.PEntryData<GUIForDiskpart.Presentation.View.UserControls.UCEntryData>;

using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.Presenter.UserControls
{
    /// <summary>
    /// Constructed with:
    /// NONE - Has no parameters!
    /// <br/><br/>
    /// Must be instanced with <c>CreateUCPresenter</c> method of a <c>WPresenter</c> derived class.<br/>
    /// If the UserControl is already present at compile time, this class should be instanced in the <c>InitPresenters</c> method. <br/>
    /// See code example:
    /// <para>
    /// <code>
    /// public override void InitPresenters()
    /// {
    ///     someProperty = CreateUCPresenter&lt;PSomething&gt;(Window.SomeUserControl);
    /// }
    /// </code>
    /// </para>
    /// </summary>
    public class PEntryData<T> : UCPresenter<T> where T : View.UserControls.UCEntryData
    {
        private void OnSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserControl.EntryDataGrid.SelectAllCells();

            List<DataGridCellInfo> cells = new List<DataGridCellInfo>();

            foreach (var entry in UserControl.EntryDataGrid.SelectedCells)
            {
                if (entry.Item.ToString().Contains(UserControl.SearchBar.Text, System.StringComparison.OrdinalIgnoreCase))
                {
                    cells.Add(entry);
                }
            }

            UserControl.EntryDataGrid.UnselectAllCells();

            if (cells.Count > 0)
            {
                foreach (var cell in cells)
                {
                    UserControl.EntryDataGrid.SelectedCells.Add(cell);
                }
            }

            if (string.IsNullOrEmpty(UserControl.SearchBar.Text))
            {
                UserControl.EntryDataGrid.UnselectAllCells();
            }
        }

        private void OnSearchBar_CursorFocus(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!UserControl.SearchBar.IsFocused)
            {
                UserControl.SearchBar.Text = "";
            }
        }

        public void OnSaveEntryData_Click(object sender, RoutedEventArgs e)
        {
            string entrieString = string.Empty;

            bool noneSelected = false;

            if (UserControl.EntryDataGrid.SelectedCells.Count == 0)
            {
                UserControl.EntryDataGrid.SelectAllCells();
                noneSelected = true;
            }

            foreach (var entry in UserControl.EntryDataGrid.SelectedCells)
            {
                if (entry.Item == null) continue;
                if (entrieString.Contains(entry.Item.ToString())) continue;
                entrieString += entry.Item + "\n";
            }

            if (noneSelected)
            {
                UserControl.EntryDataGrid.UnselectAllCells();
            }

            FileUtils.SaveAsTextfile(entrieString, "data");
        }

        private void OnEntryDataGrid_LMBUp(object sender, MouseButtonEventArgs e)
        {
            if (UserControl.EntryDataGrid.SelectedCells.Count == 1)
            {
                if (UserControl.SelectedCell == UserControl.EntryDataGrid.SelectedCells[0])
                {
                    UserControl.EntryDataGrid.UnselectAllCells();
                    UserControl.SelectedCell = null;
                    return;
                }
                UserControl.SelectedCell = UserControl.EntryDataGrid.SelectedCells[0];
            }
        }

        public void AddDataToGrid(Dictionary<string, object?> data)
        {
            UserControl.EntryDataGrid.ItemsSource = data;
        }

        #region UCPresenter

        protected override void RegisterEventsInternal()
        {
            UserControl.ESaveEntryData += OnSaveEntryData_Click;
            UserControl.ESelection += OnEntryDataGrid_LMBUp;
            UserControl.ESearchBarText += OnSearchBar_TextChanged;
            UserControl.ESearchBarFocused += OnSearchBar_CursorFocus;
        }

        #endregion UCPresenter
    }
}
