using Leopotam.EcsLite;
using TestAsssignment.Components;
using TestAsssignment.Configs;

namespace TestAsssignment.Systems
{
    public class BusinessCreateSystem : IEcsInitSystem
    {
        private BusinessConfig[] _configs;
        private const int _startBusinessLevel = 1;

        public BusinessCreateSystem(BusinessConfig[] configs)
        {
            _configs = configs;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var configPool = world.GetPool<BusinessConfigComponent>();
            var activeBusinessPool = world.GetPool<ActiveBusinessComponent>();

            foreach (var config in _configs)
            {
                var entity = world.NewEntity();

                ref BusinessConfigComponent configComponent = ref configPool.Add(entity);
                configComponent.config = config;

                if (config.ActiveOnStart)
                {
                    ref var activeBusiness = ref activeBusinessPool.Add(entity);

                    activeBusiness.businessLevel = _startBusinessLevel;
                    activeBusiness.boughtImprovements = new int[0];
                    activeBusiness.profitProgress = 0;
                }
            }
        }
    }
}