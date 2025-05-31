using System.Collections.Generic;

using GUIForDiskpart.Presentation.Presenter;


namespace GUIForDiskpart.Presentation.View.UserControls
{
    public interface IGUIFDUserControl
    {
        public List<IPresenter> Presenters => GetPresenters();
        protected List<IPresenter> GetPresenters();
    }
}
