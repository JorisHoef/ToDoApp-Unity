using System;

namespace JorisHoef.UI.HoverSystem
{
    public interface IHoverable
    {
        public event Action<IHoverable> OnSelected;
        public void SetSelection(bool isSelected);
    }
}