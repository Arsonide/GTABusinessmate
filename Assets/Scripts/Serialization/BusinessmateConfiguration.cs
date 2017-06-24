using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Arsonide
{
    [System.Serializable]
    public class BusinessmateConfiguration
    {
        public PriorityTimes SalesWarnings;
        public PriorityTimes ResupplyWarnings;
        public PriorityTimes CooldownWarnings;

        [SerializeField]
        protected BusinessConfiguration[] BusinessConfigurations;

        [SerializeField]
        protected CooldownConfiguration[] CooldownConfigurations;

        [System.NonSerialized]
        protected Dictionary<string, BusinessConfiguration> BusinessDictionary;

        [System.NonSerialized]
        protected Dictionary<string, CooldownConfiguration> CooldownDictionary;

        [System.NonSerialized]
        protected static readonly string configurationPath = "/Configuration.json";

        public static string ConfigurationFile { get { return Businessmate.ConfigurationDirectory + configurationPath; } }

        public static bool ConfigurationFileExists(bool create = false)
        {
            bool exists = true;

            if (!Directory.Exists(Businessmate.ConfigurationDirectory))
            {
                exists = false;

                if (create)
                    Directory.CreateDirectory(Businessmate.ConfigurationDirectory);
            }

            if (!File.Exists(ConfigurationFile))
            {
                exists = false;

                if (create)
                    File.Create(ConfigurationFile).Dispose();
            }

            return exists;
        }

        public static BusinessmateConfiguration LoadFromFile()
        {
            BusinessmateConfiguration configuration = Directory.Exists(Businessmate.ConfigurationDirectory) && File.Exists(ConfigurationFile) ? JsonUtility.FromJson<BusinessmateConfiguration>(File.ReadAllText(ConfigurationFile)) : null;

            if (configuration == null)
                return configuration;

            configuration.BusinessDictionary = new Dictionary<string, BusinessConfiguration>();

            if (configuration.BusinessConfigurations != null)
            {
                foreach (BusinessConfiguration businessConfiguration in configuration.BusinessConfigurations)
                {
                    configuration.BusinessDictionary.Add(businessConfiguration.BusinessID, businessConfiguration);
                }
            }

            configuration.CooldownDictionary = new Dictionary<string, CooldownConfiguration>();

            if (configuration.CooldownConfigurations != null)
            {
                foreach (CooldownConfiguration cooldownConfiguration in configuration.CooldownConfigurations)
                {
                    configuration.CooldownDictionary.Add(cooldownConfiguration.CooldownID, cooldownConfiguration);
                }
            }

            return configuration;
        }

        public BusinessConfiguration GetBusiness(string id)
        {
            BusinessConfiguration configuration;

            if (BusinessDictionary == null || !BusinessDictionary.TryGetValue(id, out configuration))
                return default(BusinessConfiguration);

            return configuration;
        }

        public CooldownConfiguration GetCooldown(string id)
        {
            CooldownConfiguration configuration;

            if (CooldownDictionary == null || !CooldownDictionary.TryGetValue(id, out configuration))
                return default(CooldownConfiguration);

            return configuration;
        }
    }
}