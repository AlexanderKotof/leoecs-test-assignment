using Leopotam.EcsLite;
using TestAsssignment.Components;
using static TestAsssignment.Startup;

namespace TestAsssignment.Systems
{
    public class AddMoneySystem : IEcsPostRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        private EcsPool<AddMoneyComponent> _addMoneyPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<AddMoneyComponent>().End();
            _addMoneyPool = world.GetPool<AddMoneyComponent>();
        }

        public void PostRun(IEcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();

            foreach (var entity in _filter)
            {
                ref var addMoney = ref _addMoneyPool.Get(entity);
                sharedData.AddMoney(addMoney.value);
                _addMoneyPool.Del(entity);
            }
        }
    }
}