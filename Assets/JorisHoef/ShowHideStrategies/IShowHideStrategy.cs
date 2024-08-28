namespace JorisHoef.ShowHideStrategies
{
    public interface IShowHideStrategy
    {
        public bool IsShown { get; }
        void Show();
        void Hide();
    }
}