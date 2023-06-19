using Leopotam.EcsLite;
using TestAsssignment.Components;
using TestAsssignment.Utils;
using UnityEngine;

namespace TestAsssignment.Systems
{
    public class BusinessProfitSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;

        private EcsPool<ActiveBusinessComponent> _activePool;
        private EcsPool<BusinessConfigComponent> _configsPool;
        private EcsPool<AddMoneyComponent> _profitPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<BusinessConfigComponent>().Inc<ActiveBusinessComponent>().End();

            _activePool = world.GetPool<ActiveBusinessComponent>();
            _configsPool = world.GetPool<BusinessConfigComponent>();
            _profitPool = world.GetPool<AddMoneyComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var activeComponent = ref _activePool.Get(entity);
                var configComponent = _configsPool.Get(entity);

                activeComponent.profitProgress += Time.deltaTime;

                if (activeComponent.profitProgress < configComponent.config.ProfitDelay)
                    continue;

                activeComponent.profitProgress = 0;
                ref var profit = ref _profitPool.Add(systems.GetWorld().NewEntity());
                profit.value = GameMathUtils.CalculateBusinessProfit(configComponent.config, activeComponent.businessLevel, activeComponent.purchasedUpgrades);
            }
        }
    }
}