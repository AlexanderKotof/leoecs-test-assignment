using Leopotam.EcsLite;
using TestAsssignment.Components;
using static TestAsssignment.Startup;

namespace TestAsssignment.Systems
{
    public class SpendMoneySystem : IEcsPostRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        private EcsPool<SpendMoneyComponent> _spendMoneyPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<SpendMoneyComponent>().End();
            _spendMoneyPool = world.GetPool<SpendMoneyComponent>();
        }

        public void PostRun(IEcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();

            foreach (var entity in _filter)
            {
                ref var spend = ref _spendMoneyPool.Get(entity);
                sharedData.SpendMoney(spend.value);
                _spendMoneyPool.Del(entity);
            }
        }
    }
}