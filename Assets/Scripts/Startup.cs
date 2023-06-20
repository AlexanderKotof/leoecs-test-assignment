using Leopotam.EcsLite;
using System;
using TestAsssignment.Configs;
using TestAsssignment.Systems;
using TestAsssignment.Utils;
using UnityEngine;
using static TestAsssignment.Utils.SaveGameUtils;

namespace TestAsssignment
{
    public class Startup : MonoBehaviour
    {
        public GameSettingsConfig gameSettings;

        public class SharedData
        {
            public double Balance { get; private set; }
            public SaveData SavedData { get; private set; }

            public bool HasSavedData => SavedData != null;

            public event Action<double> MoneyChanged;

            public SharedData(double startMoney)
            {
                Balance = startMoney;
                SavedData = null;
            }

            public SharedData(SaveData savedData)
            {
                Balance = savedData.balance;
                SavedData = savedData;
            }

            public void AddMoney(double value)
            {
                Balance += value;
                MoneyChanged?.Invoke(Balance);
            }

            public void SpendMoney(double value)
            {
                Balance -= value;
                MoneyChanged?.Invoke(Balance);
            }

            public bool HasMoney(double value)
            {
                return Balance >= value;
            }
        }
        private SharedData _sharedData;

        private EcsWorld _world;
        private IEcsSystems _systems;

        private void Start()
        {
            if (SaveGameUtils.TryLoadGameData(out var data))
            {
                _sharedData = new SharedData(data);
            }
            else
                _sharedData = new SharedData(gameSettings.StartBalance);

            _world = new EcsWorld();
            _systems = new EcsSystems(_world, _sharedData);
            _systems
                .Add(new BusinessCreateSystem(gameSettings.BusinessConfigs))
                .Add(new BusinessProfitSystem())
                .Add(new AddMoneySystem())
                .Add(new SpendMoneySystem())
                .Add(new BusinessLvlUpSystem())
                .Add(new BusinessUpgradePurchaseSystem())
                .Add(new GameUISystem(gameSettings.GameScreenPrefab))
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                .Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem())
#endif
                .Init();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnApplicationQuit()
        {
            SaveGameUtils.SaveGameData(_world, _sharedData.Balance);
        }

        private void OnApplicationFocus(bool focus)
        {
            if (!focus)
                SaveGameUtils.SaveGameData(_world, _sharedData.Balance);
        }

        private void OnDestroy()
        {
            _systems?.Destroy();
            _systems = null;
            _world?.Destroy();
            _world = null;
        }
    }
}