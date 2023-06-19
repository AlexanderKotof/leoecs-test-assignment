using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using TestAsssignment.Components;
using TestAsssignment.Configs;
using TestAsssignment.UI;
using UnityEngine;
using static TestAsssignment.Startup;

namespace TestAsssignment.Systems
{
    public class GameUISystem : IEcsInitSystem, IEcsPostRunSystem, IEcsDestroySystem
    {
        private GameScreenComponent _gameScreenPrefab;
        private GameScreenComponent _screenInstance;

        private EcsFilter _businessesFilter;

        private EcsPool<ActiveBusinessComponent> _activeBuisnessesPool;
        private EcsPool<BusinessConfigComponent> _buisnessesCongfigsPool;

        private EcsWorld _world;
        private SharedData _sharedData;

        public class BusinessData
        {
            public int entity;

            public BusinessConfig config;

            public bool[] purchasedUpgrades;

            public int lvlValue;

            public float profitProgress;

            public Action<BusinessData> onLvlUpPressed;
            public Action<BusinessData, int> onPurchaseUpgradePressed;
        }

        public GameUISystem(GameScreenComponent gameScreenPrefab)
        {
            _gameScreenPrefab = gameScreenPrefab;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _businessesFilter = _world.Filter<BusinessConfigComponent>().End();
            _buisnessesCongfigsPool = _world.GetPool<BusinessConfigComponent>();
            _activeBuisnessesPool = _world.GetPool<ActiveBusinessComponent>();

            _screenInstance = GameObject.Instantiate(_gameScreenPrefab);

            var businessDatas = GetBusnissesData();
            _screenInstance.SetInfo(businessDatas);

            _sharedData = systems.GetShared<SharedData>();
            _sharedData.MoneyChanged += _screenInstance.SetBalanceValue;
            _screenInstance.SetBalanceValue(_sharedData.Money);
        }

        private List<BusinessData> GetBusnissesData()
        {
            var businessDatas = new List<BusinessData>();

            foreach (var entity in _businessesFilter)
            {
                var data = new BusinessData
                {
                    config = _buisnessesCongfigsPool.Get(entity).config,
                    entity = entity,
                    onLvlUpPressed = OnLvlUpPressed,
                    onPurchaseUpgradePressed = OnPurchaseUpgradePressed,
                };

                if (_activeBuisnessesPool.Has(entity))
                {
                    var activeBusiness = _activeBuisnessesPool.Get(entity);

                    data.lvlValue = activeBusiness.businessLevel;
                    data.purchasedUpgrades = activeBusiness.purchasedUpgrades;
                    data.profitProgress = activeBusiness.profitProgress;
                }
                else
                {
                    data.lvlValue = 0;
                    data.purchasedUpgrades = new bool[BusinessConfig.upgradesCount];
                    data.profitProgress = 0;
                }

                businessDatas.Add(data);
            }

            return businessDatas;
        }

        public void Destroy(IEcsSystems systems)
        {
            _sharedData.MoneyChanged -= _screenInstance.SetBalanceValue;
        }

        private void OnPurchaseUpgradePressed(BusinessData data, int upgradeIndex)
        {
            Debug.Log("Purchase Upgrade click!");

            //TODO: check available balance and that level is more 0 and buy upgrade
        }

        private void OnLvlUpPressed(BusinessData data)
        {
            Debug.Log("Lvl Up click!");

            //TODO: check available balance and that level is more 0 and buy upgrade
        }

        public void PostRun(IEcsSystems systems)
        {
            _screenInstance.UpdateScreen(_activeBuisnessesPool);
        }
    }
}