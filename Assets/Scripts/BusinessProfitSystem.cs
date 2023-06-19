using Leopotam.EcsLite;
using TestAsssignment.Components;
using UnityEngine;

namespace TestAsssignment.Systems
{
    public class BusinessProfitSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;

        private EcsPool<ActiveBusinessComponent> _activePool;
        private EcsPool<BusinessConfigComponent> _configsPool;
        private EcsPool<AddMoney> _profitPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<BusinessConfigComponent>().Inc<ActiveBusinessComponent>().End();

            _activePool = world.GetPool<ActiveBusinessComponent>();
            _configsPool = world.GetPool<BusinessConfigComponent>();
            _profitPool = world.GetPool<AddMoney>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var activeComponent = ref _activePool.Get(entity);
                ref var configComponent = ref _configsPool.Get(entity);

                activeComponent.profitProgress += Time.deltaTime;

                if (activeComponent.profitProgress < configComponent.config.ProfitDelay)
                    continue;

                activeComponent.profitProgress = 0;
                ref var profit = ref _profitPool.Add(systems.GetWorld().NewEntity());
                profit.value = CalculateBusinessProfit(activeComponent, configComponent);
            }
        }

        private double CalculateBusinessProfit(ActiveBusinessComponent activeBusiness, BusinessConfigComponent configComponent)
        {
            double value = configComponent.config.BaseProfit * activeBusiness.businessLevel;
            double multiplier = 1;

            foreach (var improvement in activeBusiness.boughtImprovements)
            {
                multiplier += configComponent.config.Improvements[improvement].profitMultiplier;
            }

            return value * multiplier;
        }
    }
}