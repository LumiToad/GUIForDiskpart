global using PUnallocatedEntry =
    GUIForDiskpart.Presentation.Presenter.UserControls.PUnallocatedEntry<GUIForDiskpart.Presentation.View.UserControls.UCUnallocatedEntry>;

using System.Collections.Generic;
using System.Windows;

using GUIForDiskpart.Presentation.View.UserControls;
using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.Presenter.UserControls
{
    /// <summary>
    /// Constructed with:
    /// <value><c>DiskModel</c> Disk</value>
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
    public class PUnallocatedEntry<T> : UCPresenter<T> where T : UCUnallocatedEntry
    {
        private long size;
        public DiskModel Disk { get; private set; }

        public bool? IsSelected { get { return UserControl.EntrySelected.IsChecked; } }

        public Dictionary<string, object?> EntryData
        {
            get
            {
                Dictionary<string, object?> dict = new Dictionary<string, object?>();
                dict.Add("Unallocated Space", UserControl.Size.Content);

                return dict;
            }
        }

        public void SelectEntryRadioButton()
        {
            UserControl.EntrySelected.IsChecked = !UserControl.EntrySelected.IsChecked;
        }

        private void SetSize(string size)
        {
            UserControl.Size.Content = size;
        }

        #region OnClick

        private void OnButton_Click(object sender, RoutedEventArgs e)
        {
            SelectEntryRadioButton();
            MainWindow.OnUnallocatedEntry_Click(UserControl);
        }

        private void OnCreatePart_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PCreatePartition>(true, Disk, size);
        }

        private void OnCreateVolume_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PCreateVolume>(true, Disk, Disk.UnallocatedSpace);
        }

        private void OnOpenContextMenu_Click(object sender, RoutedEventArgs e)
        {
            UserControl.ContextMenu.IsOpen = !UserControl.ContextMenu.IsOpen;
        }

        #endregion OnClick

        #region UCPresenter

        public override void Setup()
        {
            size = Disk.UnallocatedSpace;
            SetSize(ByteFormatter.BytesToUnitAsString(size));
        }

        protected override void RegisterEventsInternal()
        {
            UserControl.EButton_Click += OnButton_Click;
            UserControl.ECreatePart_Click += OnCreatePart_Click;
            UserControl.ECreateVolume_Click += OnCreateVolume_Click;
            UserControl.EOpenContextMenu_Click += OnOpenContextMenu_Click;
        }

        public override void AddCustomArgs(params object?[] args)
        {
            Disk = (DiskModel)args[0];
        }

        #endregion UCPresenter
    }
}
