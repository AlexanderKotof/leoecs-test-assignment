using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.IO;
using TestAsssignment.Components;
using UnityEngine;

namespace TestAsssignment.Utils
{
    public static class SaveGameUtils
    {
        [Serializable]
        public class SaveData
        {
            public double balance;
            public BusinessSaveData[] businessDatas;

            [Serializable]
            public class BusinessSaveData
            {
                public string businessName;
                public int level;
                public float progress;
                public bool[] purchasedUpgrades;
            }
        }

        private const string _filename = "savedData.json";
        private static readonly string _pathFormat
#if UNITY_EDITOR
         = $"{Application.dataPath}/{_filename}";
#else
         = $"{Application.persistentDataPath}/{_filename}";
#endif

        public static  void SaveGameData(EcsWorld world, double balance)
        {
            var saveData = CreateSaveData(world, balance);
            string dataToJson = JsonUtility.ToJson(saveData);
            File.WriteAllText(_pathFormat, dataToJson);
        }

        private static SaveData CreateSaveData(EcsWorld world, double balance)
        {
            var saveData = new SaveData();
            saveData.balance = balance;

            var activeBusinessesFilter = world.Filter<BusinessConfigComponent>().Inc<ActiveBusinessComponent>().End();
            var activeBusinessPool = world.GetPool<ActiveBusinessComponent>();
            var configPool = world.GetPool<BusinessConfigComponent>();

            var businessDatas = new List<SaveData.BusinessSaveData>();

            foreach (var entity in activeBusinessesFilter)
            {
                var activeComponent = activeBusinessPool.Get(entity);
                var configComponent = configPool.Get(entity);
                businessDatas.Add(new SaveData.BusinessSaveData()
                {
                    businessName = configComponent.config.BusinesName,
                    level = activeComponent.businessLevel,
                    progress = activeComponent.profitProgress,
                    purchasedUpgrades = activeComponent.purchasedUpgrades,
                });
            }
            saveData.businessDatas = businessDatas.ToArray();

            return saveData;
        }

        public static bool TryLoadGameData(out SaveData data)
        {
            data = null;

            if (File.Exists(_pathFormat))
            {
                data = JsonUtility.FromJson<SaveData>(File.ReadAllText(_pathFormat));
                return data != null;
            }

            return false;
        }
    }
}