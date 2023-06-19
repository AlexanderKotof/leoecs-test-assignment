using Leopotam.EcsLite;
using System;
using TestAsssignment.Configs;
using TestAsssignment.Systems;
using UnityEngine;

namespace TestAsssignment
{
    public class Startup : MonoBehaviour
    {
        public GameSettingsConfig gameSettings;

        public class SharedData
        {
            public double Money { get; private set; }

            public event Action<double> MoneyChanged;

            public SharedData(double startMoney)
            {
                Money = startMoney;
            }

            public void AddMoney(double value)
            {
                Money += value;
                MoneyChanged?.Invoke(Money);
            }

            public void SpendMoney(double value)
            {
                Money -= value;
                MoneyChanged?.Invoke(Money);
            }

            public bool HasMoney(double value)
            {
                return Money >= value;
            }
        }
        private SharedData _sharedData;

        private EcsWorld _world;
        private IEcsSystems _systems;

        private void Start()
        {
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

        private void OnDestroy()
        {
            _systems?.Destroy();
            _systems = null;
            _world?.Destroy();
            _world = null;
        }
    }
}