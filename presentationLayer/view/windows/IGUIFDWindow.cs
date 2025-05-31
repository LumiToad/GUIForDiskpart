using System.Collections.Generic;
using System.Windows;

using GUIForDiskpart.Utils;
using GUIForDiskpart.Presentation.Presenter;
using GUIForDiskpart.Presentation.View.UserControls;
using System;


namespace GUIForDiskpart.Presentation.View.Windows
{
    public interface IGUIFDWindow
    {
        //public IPresenter WindowPresenter => 
        public Dictionary<IGUIFDUserControl, List<IPresenter>> ChildPresenters => this.GetPresentersInChildren();
    }

    public static class GUIFDWindowExtensions
    {
        public static void InitializeGUIFDWindow(this IGUIFDWindow guifdWindow)
        {
            Window window = (Window)guifdWindow;
            window.Focus();

            GetPresentersInChildren(guifdWindow);
        }

        public static Dictionary<IGUIFDUserControl, List<IPresenter>> GetPresentersInChildren(this IGUIFDWindow guifdWindow)
        {
            Window window = (Window)guifdWindow;
            Dictionary<IGUIFDUserControl, List<IPresenter>> retVal = new();

            foreach (var child in window.GetChildrenUserControls())
            {
                IGUIFDUserControl guifdUserControl = (IGUIFDUserControl)child;
                Console.WriteLine($"Test {guifdUserControl}");
                if (guifdUserControl == null) continue;

                retVal.Add(guifdUserControl, guifdUserControl.Presenters);
            }

            return retVal;
        }
    }
}
