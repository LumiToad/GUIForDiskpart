using GUIForDiskpart.Presentation.Presenter;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GUIForDiskpart.Presentation.View.UserControls
{
    /// <summary>
    /// Interaction logic for ConsoleReturn.xaml
    /// </summary>
    public partial class LogUI : UserControl, IGUIFDUserControl
    {
        public delegate void OnClick(object sender, RoutedEventArgs e);
        public event OnClick SaveLog_Click = delegate { };
        
        public delegate void OnTextChanged(object sender, TextChangedEventArgs e);
        public event OnTextChanged LogUI_TextChanged = delegate { };

        public LogUI()
        {
            InitializeComponent();
        }

        public void Print(string text)
        {

        }

        #region IGUIFDUserControl

        List<IPresenter> IGUIFDUserControl.GetPresenters()
        {
            List<IPresenter> presenters = new()
            {
                Presenter.LogUI.New(this)
            };
        
            return presenters;
        }

        #endregion IGUIFDUserControl
    }
}
