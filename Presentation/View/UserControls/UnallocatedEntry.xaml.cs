using GUIForDiskpart.Presentation.Presenter;
using GUIForDiskpart.Service;
using System.Collections.Generic;
using System.Management.Automation;
using System.Windows;
using System.Windows.Controls;


namespace GUIForDiskpart.Presentation.View.UserControls
{
    /// <summary>
    /// Interaction logic for UCUnallocatedEntry.xaml
    /// </summary>
    public partial class UCUnallocatedEntry : UserControl
    {
        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick EButton_Click;
        public event DOnClick ECreatePart_Click;
        public event DOnClick ECreateVolume_Click;
        public event DOnClick EOpenContextMenu_Click;

        public UCUnallocatedEntry()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) => EButton_Click?.Invoke(sender, e);
        private void CreatePart_Click(object sender, RoutedEventArgs e) => ECreatePart_Click?.Invoke(sender, e);
        private void CreateVolume_Click(object sender, RoutedEventArgs e) => ECreateVolume_Click?.Invoke(sender, e);
        private void OpenContextMenu_Click(object sender, RoutedEventArgs e) => EOpenContextMenu_Click?.Invoke(sender, e);
    }
}
