using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Arsonide
{
    [System.Serializable]
    public class BusinessmateSave
    {
        [SerializeField]
        protected BusinessSave[] BusinessSaves;

        [SerializeField]
        protected CooldownSave[] CooldownSaves;

        [System.NonSerialized]
        protected Dictionary<string, BusinessSave> BusinessDictionary;

        [System.NonSerialized]
        protected Dictionary<string, CooldownSave> CooldownDictionary;

        [System.NonSerialized]
        protected static readonly string savePath = "/Save.json";

        public static string SaveFile { get { return Businessmate.SaveDirectory + savePath; } }

        public static bool SaveFileExists(bool create = false)
        {
            bool exists = true;

            if (!Directory.Exists(Businessmate.SaveDirectory))
            {
                exists = false;

                if (create)
                    Directory.CreateDirectory(Businessmate.SaveDirectory);
            }

            if (!File.Exists(SaveFile))
            {
                exists = false;

                if (create)
                    File.Create(SaveFile).Dispose();
            }

            return exists;
        }

        public static void SaveDefaultFile(Business[] businesses, Cooldown[] cooldowns)
        {
            if (SaveFileExists(true))
                return;

            BusinessmateSave defaultSave = new BusinessmateSave { BusinessSaves = new BusinessSave[businesses.Length], CooldownSaves = new CooldownSave[cooldowns.Length]};

            for (int i = 0; i < businesses.Length; ++i)
            {
                businesses[i].Save = new BusinessSave { BusinessID = businesses[i].BusinessID };
                businesses[i].SaveBusiness();
                defaultSave.BusinessSaves[i] = businesses[i].Save;
            }

            for (int i = 0; i < cooldowns.Length; ++i)
            {
                cooldowns[i].Save = new CooldownSave { CooldownID = cooldowns[i].CooldownID };
                cooldowns[i].SaveCooldown();
                defaultSave.CooldownSaves[i] = cooldowns[i].Save;
            }

            defaultSave.SaveToFile();
        }

        public void SaveToFile()
        {
            if (Directory.Exists(Businessmate.SaveDirectory))
                File.WriteAllText(SaveFile, JsonUtility.ToJson(this));
        }

        public static BusinessmateSave LoadFromFile()
        {
            BusinessmateSave save = Directory.Exists(Businessmate.SaveDirectory) && File.Exists(SaveFile) ? JsonUtility.FromJson<BusinessmateSave>(File.ReadAllText(SaveFile)) : null;

            if (save == null)
                return save;

            save.BusinessDictionary = new Dictionary<string, BusinessSave>();

            if (save.BusinessSaves != null)
            {
                foreach (BusinessSave businessSave in save.BusinessSaves)
                {
                    save.BusinessDictionary.Add(businessSave.BusinessID, businessSave);
                }
            }

            save.CooldownDictionary = new Dictionary<string, CooldownSave>();

            if (save.CooldownSaves != null)
            {
                foreach (CooldownSave cooldownSave in save.CooldownSaves)
                {
                    save.CooldownDictionary.Add(cooldownSave.CooldownID, cooldownSave);
                }
            }

            return save;
        }

        public BusinessSave GetBusiness(string id)
        {
            BusinessSave save;

            if (BusinessDictionary == null || !BusinessDictionary.TryGetValue(id, out save))
                return default(BusinessSave);

            return save;
        }

        public CooldownSave GetCooldown(string id)
        {
            CooldownSave save;

            if (CooldownDictionary == null || !CooldownDictionary.TryGetValue(id, out save))
                return default(CooldownSave);

            return save;
        }
    }
}