using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using TestAsssignment.Components;
using TestAsssignment.Configs;
using TestAsssignment.UI;
using TestAsssignment.Utils;
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

            public EcsPool<ActiveBusinessComponent> activeBuisnesses;

            public Action<BusinessData> onLvlUpPressed;
            public Action<BusinessData, int> onPurchaseUpgradePressed;

            public bool IsActiveBusiness => activeBuisnesses.Has(entity);

            public ActiveBusinessComponent GetActiveBusiness => activeBuisnesses.Get(entity);
        }
        private List<BusinessData> businessDatas;

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

            businessDatas = GetBusinessDatas();
            _screenInstance.SetInfo(businessDatas);

            _sharedData = systems.GetShared<SharedData>();
            _sharedData.MoneyChanged += _screenInstance.SetBalanceValue;
            _screenInstance.SetBalanceValue(_sharedData.Balance);
        }

        private List<BusinessData> GetBusinessDatas()
        {
            var businessDatas = new List<BusinessData>();

            foreach (var entity in _businessesFilter)
            {
                var data = new BusinessData
                {
                    config = _buisnessesCongfigsPool.Get(entity).config,
                    entity = entity,
                    activeBuisnesses = _activeBuisnessesPool,
                    onLvlUpPressed = OnLvlUpPressed,
                    onPurchaseUpgradePressed = OnPurchaseUpgradePressed,
                };

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
            if (!data.IsActiveBusiness)
                return;

            var requiredMoney = data.config.Upgrades[upgradeIndex].upgradePrice;

            if (!_sharedData.HasMoney(requiredMoney))
                return;

            var entity = _world.NewEntity();

            var spendMoneyPool = _world.GetPool<SpendMoneyComponent>();
            var upgradePool = _world.GetPool<BuyBusinessUpgradeCommponent>();

            ref var spend = ref spendMoneyPool.Add(entity);
            spend.value = requiredMoney;

            ref var upgrade = ref upgradePool.Add(entity);
            upgrade.entity = data.entity;
            upgrade.index = upgradeIndex;
        }

        private void OnLvlUpPressed(BusinessData data)
        {
            int level = 0;
            if (data.IsActiveBusiness)
            {
                level = data.GetActiveBusiness.businessLevel;
            }

            var requiredMoney = GameMathUtils.GetBusinessLvlUpPrice(data.config, level);

            if (!_sharedData.HasMoney(requiredMoney))
                return;

            var entity = _world.NewEntity();

            var spendMoneyPool = _world.GetPool<SpendMoneyComponent>();
            var lvlupPool = _world.GetPool<IncreaseBusinessLevelComponent>();

            ref var spend = ref spendMoneyPool.Add(entity);
            spend.value = requiredMoney;

            ref var upgrade = ref lvlupPool.Add(entity);
            upgrade.entity = data.entity;
        }

        public void PostRun(IEcsSystems systems)
        {
            _screenInstance.UpdateInfo();
        }
    }
}