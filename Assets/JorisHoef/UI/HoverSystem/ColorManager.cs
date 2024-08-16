using UnityEngine;

namespace JorisHoef.UI.HoverSystem
{
    public class ColorManager
    {
        private ColorManager() { }
        public static ColorManager Instance { get; } = new ColorManager();

        public Color GetColor(Color selectedMaterial)
        {
            return selectedMaterial;
        }
    }
}