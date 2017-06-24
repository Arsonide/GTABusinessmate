using UnityEngine.UI;

namespace Arsonide
{
    public interface ISliderEventListener
    {
        void OnSliderBeginDrag(Slider slider);
        void OnSliderEndDrag(Slider slider);
    }
}