namespace GUIForDiskpart.Presentation.Presenter
{
    public interface IPresenter
    {
        public static IPresenter? New(params object[] args) { return default; }

        public abstract void RegisterEvents();
    }
}
