using Leopotam.EcsLite;
using TestAsssignment.Components;
using TestAsssignment.Configs;

namespace TestAsssignment.Systems
{
    public class BusinessLvlUpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;

        private EcsPool<ActiveBusinessComponent> _activePool;
        private EcsPool<IncreaseBusinessLevelComponent> _lvlUpPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<IncreaseBusinessLevelComponent>().End();

            _activePool = world.GetPool<ActiveBusinessComponent>();
            _lvlUpPool = world.GetPool<IncreaseBusinessLevelComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var lvlUpEntity = _lvlUpPool.Get(entity).entity;
                if (_activePool.Has(lvlUpEntity))
                {
                    ref var activeBusiness = ref _activePool.Get(lvlUpEntity);
                    activeBusiness.businessLevel++;
                }
                else
                {
                    ref var activeBusiness = ref _activePool.Add(lvlUpEntity);
                    activeBusiness.businessLevel = 1;
                    activeBusiness.profitProgress = 0;
                    activeBusiness.purchasedUpgrades = new bool[BusinessConfig.upgradesCount];
                }

                _lvlUpPool.Del(entity);
            }
        }
    }
}