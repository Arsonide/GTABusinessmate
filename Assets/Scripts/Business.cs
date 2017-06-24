using System;
using UnityEngine;
using UnityEngine.UI;

namespace Arsonide
{
    public class Business : MonoBehaviour, ISliderEventListener
    {
        public string BusinessID;

        [NonSerialized]
        public BusinessConfiguration Configuration;

        [NonSerialized]
        public BusinessSave Save;

        protected float NormalizedProductGeneration;
        protected float NormalizedSupplyUsage;

        protected bool IsDraggingProduct;
        protected bool IsDraggingSupply;

        #region Properties

        public bool ShouldUpdate { get { return ActiveToggle == null || ActiveToggle.isOn; } }

        public Priority SalePriority
        {
            get
            {
                if (!ShouldUpdate)
                    return Priority.None;

                float productValue = ProductSlider != null ? ProductSlider.normalizedValue : 0f;

                if (Businessmate.Instance != null && Businessmate.Instance.Configuration != null)
                {
                    if (productValue + (Businessmate.Instance.Configuration.SalesWarnings.HighPriorityTime * NormalizedProductGeneration) > Configuration.SaleThreshold)
                        return Priority.High;
                    else if (productValue + (Businessmate.Instance.Configuration.SalesWarnings.LowPriorityTime * NormalizedProductGeneration) > Configuration.SaleThreshold)
                        return Priority.Low;
                }

                return Priority.None;
            }
        }

        public Priority ResupplyPriority
        {
            get
            {
                if (!ShouldUpdate)
                    return Priority.None;

                float supplyValue = SupplySlider != null ? SupplySlider.normalizedValue : 0f;

                if (Businessmate.Instance != null && Businessmate.Instance.Configuration != null)
                {
                    if (supplyValue - (Businessmate.Instance.Configuration.ResupplyWarnings.HighPriorityTime * NormalizedProductGeneration) <= 0f)
                        return Priority.High;
                    else if (supplyValue - (Businessmate.Instance.Configuration.ResupplyWarnings.LowPriorityTime * NormalizedProductGeneration) <= 0f)
                        return Priority.Low;
                }

                return Priority.None;
            }
        }

        #endregion

        #region Components

        protected Text TitleText;
        protected Image BackgroundImage;
        protected Slider ProductSlider;
        protected Slider SupplySlider;
        protected Toggle ActiveToggle;
        protected Button SellButton;
        protected Button ResupplyButton;
        protected SliderMarker ProductMarker;
        protected SliderMarker SupplyMarker;

        protected void CacheComponents(bool cache)
        {
            if (cache)
            {
                TitleText = this.GetComponentInChildrenByName<Text>("TitleText");
                BackgroundImage = this.GetComponentInChildrenByName<Image>("Background");
                ProductSlider = this.GetComponentInChildrenByName<Slider>("ProductSlider");
                SupplySlider = this.GetComponentInChildrenByName<Slider>("SupplySlider");
                ActiveToggle = this.GetComponentInChildrenByName<Toggle>("ActiveToggle");
                SellButton = this.GetComponentInChildrenByName<Button>("SellButton");
                ResupplyButton = this.GetComponentInChildrenByName<Button>("ResupplyButton");
                ProductMarker = this.GetComponentInChildrenByName<SliderMarker>("ProductMarker");
                SupplyMarker = this.GetComponentInChildrenByName<SliderMarker>("SupplyMarker");
            }
            else
            {
                TitleText = null;
                BackgroundImage = null;
                ProductSlider = null;
                SupplySlider = null;
                ActiveToggle = null;
                SellButton = null;
                ResupplyButton = null;
                ProductMarker = null;
                SupplyMarker = null;
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
            SaveBusiness();
            SetListeners(false);
            CacheComponents(false);
        }

        #endregion

        #region Updates

        public void UpdateBusiness(float deltaTime)
        {
            if (ProductSlider != null && SupplySlider != null)
            {
                if (ProductSlider.normalizedValue < 1f)
                    SupplySlider.normalizedValue = Mathf.Clamp01(SupplySlider.normalizedValue - NormalizedSupplyUsage * deltaTime);

                if (SupplySlider.normalizedValue > 0f)
                    ProductSlider.normalizedValue = Mathf.Clamp01(ProductSlider.normalizedValue + NormalizedProductGeneration * deltaTime);
            }
        }

        public void UpdateGUI()
        {
            if (IsDraggingProduct || IsDraggingSupply)
                UpdateTitle();
        }

        #endregion

        #region Serialization

        public void SaveBusiness()
        {
            if (Save != null)
            {
                Save.IsActive = ActiveToggle != null ? ActiveToggle.isOn : true;
                Save.ProductionCurrent = ProductSlider != null ? ProductSlider.normalizedValue : 0f;
                Save.SupplyCurrent = SupplySlider != null ? SupplySlider.normalizedValue : 0f;
            }
        }

        public void LoadBusiness(BusinessConfiguration configuration, BusinessSave save)
        {
            Configuration = configuration != null ? configuration : new BusinessConfiguration();
            Save = save != null ? save : new BusinessSave();

            if (Configuration != null)
            {
                NormalizedProductGeneration = CalculateNormalizedTickRate(Configuration.ProductionAmount, Configuration.ProductionMaximum, Configuration.ProductionRate);
                NormalizedSupplyUsage = CalculateNormalizedTickRate(Configuration.SupplyAmount, Configuration.SupplyMaximum, Configuration.SupplyRate);
            }

            if (Save != null)
            {
                SetActive(Save.IsActive, true);

                if (ProductSlider != null)
                    ProductSlider.normalizedValue = Save.ProductionCurrent;

                if (SupplySlider != null)
                    SupplySlider.normalizedValue = Save.SupplyCurrent;
            }

            UpdateTitle();
            SetupSliderMarkers();
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

            if (SellButton != null && SellButton.onClick != null)
            {
                if (active)
                    SellButton.onClick.AddListener(OnSellClicked);
                else
                    SellButton.onClick.RemoveListener(OnSellClicked);
            }

            if (ResupplyButton != null && ResupplyButton.onClick != null)
            {
                if (active)
                    ResupplyButton.onClick.AddListener(OnResupplyClicked);
                else
                    ResupplyButton.onClick.RemoveListener(OnResupplyClicked);
            }
        }

        protected void OnActiveToggleClicked(bool active)
        {
            SetActive(active, false);
        }

        protected void OnSellClicked()
        {
            if (ProductSlider != null)
                ProductSlider.normalizedValue = 0f;
        }

        protected void OnResupplyClicked()
        {
            if (SupplySlider != null)
                SupplySlider.normalizedValue = 1f;
        }

        #endregion

        #region ISliderEventListener

        public void OnSliderBeginDrag(Slider slider)
        {
            if (slider == ProductSlider)
                IsDraggingProduct = true;
            else if (slider == SupplySlider)
                IsDraggingSupply = true;

            UpdateTitle();
        }

        public void OnSliderEndDrag(Slider slider)
        {
            if (slider == ProductSlider)
                IsDraggingProduct = false;
            else if (slider == SupplySlider)
                IsDraggingSupply = false;

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

            if (ProductSlider != null)
                ProductSlider.interactable = active;

            if (SupplySlider != null)
                SupplySlider.interactable = active;

            if (SellButton != null)
                SellButton.interactable = active;

            if (ResupplyButton != null)
                ResupplyButton.interactable = active;
        }

        protected void UpdateTitle()
        {
            if (TitleText == null)
                return;

            string title = Configuration != null ? Configuration.FacilityName : "Business";

            if (IsDraggingProduct)
            {
                float salePrice = Configuration != null ? Configuration.SalePriceClose : 0f;
                float value = ProductSlider != null ? ProductSlider.normalizedValue * salePrice : 0f;
                value = value.ToNearest(1000f);
                title = string.Format("Close Sale: {0:C0}", value);
            }
            else if (IsDraggingSupply)
            {
                float value = SupplySlider != null ? SupplySlider.normalizedValue * 5f : 0f;
                value = value.ToNearest(0.5f);
                title = string.Format("Supply: {0} Bars", value);
            }

            TitleText.text = string.Format("<b>{0}</b>", title);
        }

        protected void SetupSliderMarkers()
        {
            if (ProductMarker != null)
            {
                ProductMarker.xRatio = Configuration.SaleThreshold;
                ProductMarker.flip = false;
            }

            if (SupplyMarker != null)
            {
                SupplyMarker.xRatio = Configuration.ResupplyThreshold;
                SupplyMarker.flip = true;
            }
        }

        protected float CalculateNormalizedTickRate(float tickAmount, float maximumAmount, float tickRate)
        {
            float normalizedTickAmount = tickAmount / maximumAmount;
            return normalizedTickAmount / tickRate;
        }

        #endregion
    }
}