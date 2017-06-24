using System;
using UnityEngine;
using UnityEngine.UI;

namespace Arsonide
{
    public class Cooldown : MonoBehaviour, ISliderEventListener
    {
        public string CooldownID;

        [NonSerialized]
        public CooldownConfiguration Configuration;

        [NonSerialized]
        public CooldownSave Save;

        protected float NormalizedCooldownDelta;

        protected bool IsDraggingCooldown;

        #region Properties

        public bool ShouldUpdate { get { return ActiveToggle == null || ActiveToggle.isOn; } }

        public Priority CooldownPriority
        {
            get
            {
                if (!ShouldUpdate)
                    return Priority.None;

                float cooldownValue = CooldownSlider != null ? CooldownSlider.normalizedValue : 0f;

                if (Businessmate.Instance != null && Businessmate.Instance.Configuration != null)
                {
                    if (cooldownValue - (Businessmate.Instance.Configuration.CooldownWarnings.HighPriorityTime * NormalizedCooldownDelta) <= 0f)
                        return Priority.High;
                    else if (cooldownValue - (Businessmate.Instance.Configuration.CooldownWarnings.LowPriorityTime * NormalizedCooldownDelta) <= 0f)
                        return Priority.Low;
                }

                return Priority.None;
            }
        }

        #endregion

        #region Components

        protected Text TitleText;
        protected Image BackgroundImage;
        protected Toggle ActiveToggle;
        protected Slider CooldownSlider;
        protected Button ResetButton;

        protected void CacheComponents(bool cache)
        {
            if (cache)
            {
                TitleText = this.GetComponentInChildrenByName<Text>("TitleText");
                BackgroundImage = this.GetComponentInChildrenByName<Image>("Background");
                ActiveToggle = this.GetComponentInChildrenByName<Toggle>("ActiveToggle");
                CooldownSlider = this.GetComponentInChildrenByName<Slider>("CooldownSlider");
                ResetButton = this.GetComponentInChildrenByName<Button>("ResetButton");
            }
            else
            {
                TitleText = null;
                BackgroundImage = null;
                ActiveToggle = null;
                CooldownSlider = null;
                ResetButton = null;
            }
        }

        #endregion

        #region Unity

        protected void OnEnable()
        {
            CacheComponents(true);
            SetListeners(true);
        }

        protected void OnDisable()
        {
            SaveCooldown();
            SetListeners(false);
            CacheComponents(false);
        }

        #endregion

        #region Updates

        public void UpdateCooldown(float deltaTime)
        {
            if (CooldownSlider != null)
                CooldownSlider.normalizedValue -= NormalizedCooldownDelta * deltaTime;
        }

        public void UpdateGUI()
        {
            if (IsDraggingCooldown)
                UpdateTitle();
        }

        #endregion

        #region Serialization

        public void SaveCooldown()
        {
            if (Save != null)
                Save.IsActive = ActiveToggle != null ? ActiveToggle.isOn : true;
        }

        public void LoadCooldown(CooldownConfiguration configuration, CooldownSave save)
        {
            Configuration = configuration != null ? configuration : new CooldownConfiguration();
            Save = save != null ? save : new CooldownSave();

            if (Configuration != null)
                NormalizedCooldownDelta = 1f / Configuration.CooldownMaximum;

            if (Save != null)
                SetActive(Save.IsActive, true);

            UpdateTitle();
        }

        #endregion

        #region Events

        protected void SetListeners(bool active)
        {
            if (ActiveToggle != null && ActiveToggle.onValueChanged != null)
            {
                if (active)
                    ActiveToggle.onValueChanged.AddListener(OnActiveToggleClicked);
                else
                    ActiveToggle.onValueChanged.RemoveListener(OnActiveToggleClicked);
            }

            if (ResetButton != null && ResetButton.onClick != null)
            {
                if (active)
                    ResetButton.onClick.AddListener(OnResetClicked);
                else
                    ResetButton.onClick.RemoveListener(OnResetClicked);
            }
        }

        protected void OnActiveToggleClicked(bool active)
        {
            SetActive(active, false);
        }

        protected void OnResetClicked()
        {
            if (CooldownSlider != null)
                CooldownSlider.normalizedValue = 1f;
        }

        #endregion

        #region ISliderEventListener

        public void OnSliderBeginDrag(Slider slider)
        {
            if (slider == CooldownSlider)
                IsDraggingCooldown = true;

            UpdateTitle();
        }

        public void OnSliderEndDrag(Slider slider)
        {
            if (slider == CooldownSlider)
                IsDraggingCooldown = false;

            UpdateTitle();
        }

        #endregion

        #region Miscellaneous

        protected void SetActive(bool active, bool setToggle = false)
        {
            if (setToggle && ActiveToggle != null)
                ActiveToggle.isOn = active;

            if (BackgroundImage != null)
            {
                float shade = active ? 0.55f : 0.35f;
                BackgroundImage.color = new Color(shade, shade, shade, 1f);
            }

            if (CooldownSlider != null)
                CooldownSlider.interactable = active;

            if (ResetButton != null)
                ResetButton.interactable = active;
        }

        protected void UpdateTitle()
        {
            if (TitleText == null)
                return;

            string title = Configuration != null ? Configuration.CooldownName : "Cooldown";

            if (IsDraggingCooldown)
            {
                float maximum = Configuration != null ? Configuration.CooldownMaximum : 0f;
                TimeSpan span = TimeSpan.FromSeconds(CooldownSlider.normalizedValue * maximum);
                title = string.Format("Cooldown: {0}:{1}:{2}", span.Hours.ToString("00"), span.Minutes.ToString("00"), span.Seconds.ToString("00"));
            }

            TitleText.text = string.Format("<b>{0}</b>", title);
        }

        #endregion
    }
}