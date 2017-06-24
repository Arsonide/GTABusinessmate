using UnityEngine;
using UnityEngine.UI;

namespace Arsonide
{
    public class SliderMarker : MonoBehaviour
    {
        public float xRatio = 0.25f;
        public bool flip = false;

        protected RectTransform rect;
        protected Slider slider;
        protected Image background;

        protected void Start()
        {
            rect = this.GetComponent<RectTransform>();
            slider = this.GetComponentUpwards<Slider>();

            if (slider != null)
                background = slider.GetComponentInChildrenByName<Image>("Background");

            if (rect == null || slider == null || background == null)
                gameObject.SetActive(false);
        }

        protected void LateUpdate()
        {
            rect.sizeDelta = slider.handleRect.sizeDelta;

            Vector3[] v = new Vector3[4];
            background.rectTransform.GetWorldCorners(v);

            rect.position = new Vector3(v[0].x + (background.rectTransform.rect.width * (xRatio)), slider.handleRect.position.y, slider.handleRect.position.z);
            rect.rotation = Quaternion.Euler(flip ? 180 : 0, 0, 0);
        }
    }
}