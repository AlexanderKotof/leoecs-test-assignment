using Leopotam.EcsLite;
using TestAsssignment.Components;

namespace TestAsssignment.Systems
{
    public class BusinessUpgradePurchaseSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;

        private EcsPool<ActiveBusinessComponent> _activePool;
        private EcsPool<BuyBusinessUpgradeCommponent> _buyUpgradePool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<BuyBusinessUpgradeCommponent>().End();

            _activePool = world.GetPool<ActiveBusinessComponent>();
            _buyUpgradePool = world.GetPool<BuyBusinessUpgradeCommponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var buyUpgrade = _buyUpgradePool.Get(entity);

                ref var activeBusiness = ref _activePool.Get(buyUpgrade.entity);
                activeBusiness.purchasedUpgrades[buyUpgrade.index] = true;

                _buyUpgradePool.Del(entity);
            }
        }
    }
}