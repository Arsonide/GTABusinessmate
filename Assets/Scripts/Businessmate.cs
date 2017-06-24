using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Arsonide
{
    public class Businessmate : MonoBehaviour
    {
        public static Businessmate Instance;

        [NonSerialized]
        public BusinessmateConfiguration Configuration;

        [NonSerialized]
        public BusinessmateSave Save;

        public float TimeMultiplier = 1;
        protected bool ShouldUpdate = false;
        protected Coroutine ActivityRoutine;
        protected Color TitleColor;

        #region Properties

        public static string ConfigurationDirectory { get { return Application.dataPath; } }
        public static string SaveDirectory { get { return Application.persistentDataPath; } }

        #endregion

        #region Components

        protected Text BusinessmateTitle;
        protected Text BusinessmateActivity;
        protected Button BusinessmateStartButton;
        protected Button BusinessmateStopButton;
        protected AudioSource BusinessmateAudio;
        protected Business[] businesses;
        protected Cooldown[] cooldowns;

        protected void CacheComponents(bool cache)
        {
            if (cache)
            {
                BusinessmateTitle = this.GetComponentInChildrenByName<Text>("BusinessmateTitle");
                BusinessmateActivity = this.GetComponentInChildrenByName<Text>("BusinessmateActivity");
                BusinessmateStartButton = this.GetComponentInChildrenByName<Button>("BusinessmateStartButton");
                BusinessmateStopButton = this.GetComponentInChildrenByName<Button>("BusinessmateStopButton");
                BusinessmateAudio = this.GetComponent<AudioSource>();
                businesses = FindObjectsOfType<Business>();
                cooldowns = FindObjectsOfType<Cooldown>();
            }
            else
            {
                BusinessmateTitle = null;
                BusinessmateActivity = null;
                BusinessmateStartButton = null;
                BusinessmateStopButton = null;
                BusinessmateAudio = null;
                businesses = null;
                cooldowns = null;
            }
        }

        #endregion

        #region Unity

        protected void OnEnable()
        {
            if (Instance == null)
                Instance = this;

            CacheComponents(true);
            SetListeners(true);
            LoadData();

            if (ActivityRoutine == null)
                ActivityRoutine = StartCoroutine(SuggestedActivityRoutine());

            Application.runInBackground = true;
            SetActive(false);
        }

        protected void OnDisable()
        {
            if (ActivityRoutine != null)
            {
                StopCoroutine(ActivityRoutine);
                ActivityRoutine = null;
            }

            SaveData();
            SetListeners(false);
            CacheComponents(false);

            if (Instance == this)
                Instance = null;
        }

        protected void Update()
        {
            if (ShouldUpdate)
            {
                float deltaTime = Time.deltaTime * TimeMultiplier;

                foreach (Business business in businesses)
                {
                    if (business != null && business.ShouldUpdate)
                        business.UpdateBusiness(deltaTime);
                }

                foreach (Cooldown cooldown in cooldowns)
                {
                    if (cooldown != null && cooldown.ShouldUpdate)
                        cooldown.UpdateCooldown(deltaTime);
                }
            }

            foreach (Business business in businesses)
            {
                if (business != null)
                    business.UpdateGUI();
            }

            foreach (Cooldown cooldown in cooldowns)
            {
                if (cooldown != null)
                    cooldown.UpdateGUI();
            }

            if (BusinessmateTitle != null)
                BusinessmateTitle.color = Color.Lerp(BusinessmateTitle.color, TitleColor, Time.deltaTime * 10f);

            if (BusinessmateActivity != null)
                BusinessmateActivity.color = Color.Lerp(BusinessmateActivity.color, TitleColor, Time.deltaTime * 10f);
        }

        #endregion

        #region Serialization

        public void SaveData()
        {
            Save.SaveToFile();
        }

        public void LoadData()
        {
            Configuration = BusinessmateConfiguration.LoadFromFile() ?? new BusinessmateConfiguration();

            BusinessmateSave.SaveDefaultFile(businesses, cooldowns);
            Save = BusinessmateSave.LoadFromFile() ?? new BusinessmateSave();

            foreach (Business business in businesses)
            {
                if (business != null && Configuration != null && Save != null)
                    business.LoadBusiness(Configuration.GetBusiness(business.BusinessID), Save.GetBusiness(business.BusinessID));
            }

            foreach (Cooldown cooldown in cooldowns)
            {
                if (cooldown != null && Configuration != null && Save != null)
                    cooldown.LoadCooldown(Configuration.GetCooldown(cooldown.CooldownID), Save.GetCooldown(cooldown.CooldownID));
            }

            Array.Sort(businesses, (i1, i2) => -i1.Configuration.Priority.CompareTo(i2.Configuration.Priority));
            Array.Sort(cooldowns, (i1, i2) => -i1.Configuration.Priority.CompareTo(i2.Configuration.Priority));
        }

        #endregion

        #region Events

        protected void SetListeners(bool active)
        {
            if (BusinessmateStartButton != null && BusinessmateStartButton.onClick != null)
            {
                if (active)
                    BusinessmateStartButton.onClick.AddListener(OnBusinessmateStartClicked);
                else
                    BusinessmateStartButton.onClick.RemoveListener(OnBusinessmateStartClicked);
            }

            if (BusinessmateStopButton != null && BusinessmateStopButton.onClick != null)
            {
                if (active)
                    BusinessmateStopButton.onClick.AddListener(OnBusinessmateStopClicked);
                else
                    BusinessmateStopButton.onClick.RemoveListener(OnBusinessmateStopClicked);
            }
        }

        protected void OnBusinessmateStartClicked()
        {
            SetActive(true);
        }

        protected void OnBusinessmateStopClicked()
        {
            SetActive(false);
        }

        #endregion

        #region Activities

        protected IEnumerator SuggestedActivityRoutine()
        {
            while (this)
            {
                if (BusinessmateActivity != null)
                {
                    string suggestedActivity = string.Format("<b>Suggested Activity: {0}</b>", DetermineSuggestedActivity());

                    if (BusinessmateActivity.text != suggestedActivity)
                    {
                        BusinessmateActivity.text = suggestedActivity;

                        if (ShouldUpdate && Time.frameCount > 1)
                        {
                            if (BusinessmateAudio != null && BusinessmateAudio.clip != null && !BusinessmateAudio.isPlaying)
                                BusinessmateAudio.Play();
                        }
                    }
                }

                yield return new WaitForSeconds(1f);
            }
        }

        protected string DetermineSuggestedActivity()
        {
            foreach (Business business in businesses)
            {
                if (business != null && business.SalePriority >= Priority.High)
                {
                    string facilityName;
                    string productName;

                    if (business.Configuration != null)
                    {
                        facilityName = business.Configuration.FacilityName;
                        productName = business.Configuration.ProductName;
                    }
                    else
                    {
                        facilityName = "Facility";
                        productName = "Product";
                    }

                    return string.Format(business.ResupplyPriority >= Priority.Low ? "Resupply {0} and Sell {1}" : "Sell {1} from {0}", facilityName, productName);
                }
            }

            foreach (Business business in businesses)
            {
                if (business != null && business.ResupplyPriority >= Priority.High)
                {
                    string facilityName = business.Configuration != null ? business.Configuration.FacilityName : "Facility";
                    return string.Format("Resupply {0}", facilityName);
                }
            }

            foreach (Business business in businesses)
            {
                if (business != null && business.SalePriority >= Priority.Low)
                {
                    string facilityName;
                    string productName;

                    if (business.Configuration != null)
                    {
                        facilityName = business.Configuration.FacilityName;
                        productName = business.Configuration.ProductName;
                    }
                    else
                    {
                        facilityName = "Facility";
                        productName = "Product";
                    }

                    return string.Format(business.ResupplyPriority >= Priority.Low ? "Resupply {0} and Sell {1}" : "Sell {1} from {0}", facilityName, productName);
                }
            }

            foreach (Cooldown cooldown in cooldowns)
            {
                if (cooldown != null && cooldown.CooldownPriority >= Priority.High)
                    return cooldown.Configuration != null ? cooldown.Configuration.CooldownName : "Cooldown";
            }

            foreach (Business business in businesses)
            {
                if (business != null && business.ResupplyPriority >= Priority.Low)
                {
                    string facilityName = business.Configuration != null ? business.Configuration.FacilityName : "Facility";
                    return string.Format("Resupply {0}", facilityName);
                }
            }

            foreach (Cooldown cooldown in cooldowns)
            {
                if (cooldown != null && cooldown.CooldownPriority >= Priority.Low)
                    return cooldown.Configuration != null ? cooldown.Configuration.CooldownName : "Cooldown";
            }

            return "VIP Work / Crates / Imports / Fill Garages";
        }

        #endregion

        #region Miscellaneous

        protected void SetActive(bool active)
        {
            ShouldUpdate = active;
            TitleColor = active ? new Color(0.486f, 0.824f, 0.490f, 1f) : new Color(0.824f, 0.486f, 0.486f, 1f);

            if (BusinessmateStartButton != null)
                BusinessmateStartButton.interactable = !active;

            if (BusinessmateStopButton != null)
                BusinessmateStopButton.interactable = active;
        }

        #endregion
    }
}