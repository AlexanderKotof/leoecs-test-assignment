using Leopotam.EcsLite;
using System;
using TestAsssignment.Configs;
using TestAsssignment.Systems;
using TestAsssignment.UI;
using UnityEngine;

namespace TestAsssignment
{
    public class Startup : MonoBehaviour
    {
        [Serializable]
        public class Businesess
        {
            public BusinessConfig[] data;
        }
        public Businesess businesses;

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
        }
        private SharedData _sharedData;

        public GameScreenComponent gameScreenPrefab;

        private EcsWorld _world;
        private IEcsSystems _systems;

        void Start()
        {
            _sharedData = new SharedData(100);

            _world = new EcsWorld();
            _systems = new EcsSystems(_world, _sharedData);
            _systems
                .Add(new BusinessCreateSystem(businesses.data))
                .Add(new BusinessProfitSystem())
                .Add(new AddMoneySystem())
                .Add(new SpendMoneySystem())
                .Add(new GameUISystem(gameScreenPrefab))
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
                .Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem())
#endif
                .Init();
        }

        void Update()
        {
            _systems?.Run();
        }

        void OnDestroy()
        {
            _systems?.Destroy();
            _systems = null;
            _world?.Destroy();
            _world = null;
        }
    }
}