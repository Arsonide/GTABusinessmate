using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Arsonide
{
    public class SliderEventDispatcher : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        protected ISliderEventListener listener;
        protected Slider slider;

        protected void Start()
        {
            listener = this.GetComponentUpwards<ISliderEventListener>();
            slider = this.GetComponent<Slider>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (listener != null && slider != null)
                listener.OnSliderBeginDrag(slider);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (listener != null && slider != null)
                listener.OnSliderEndDrag(slider);
        }
    }
}
