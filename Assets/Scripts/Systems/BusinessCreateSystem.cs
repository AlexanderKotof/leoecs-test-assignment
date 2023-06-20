using Leopotam.EcsLite;
using TestAsssignment.Components;
using TestAsssignment.Configs;
using static TestAsssignment.Startup;

namespace TestAsssignment.Systems
{
    public class BusinessCreateSystem : IEcsInitSystem
    {
        private BusinessConfig[] _configs;

        public BusinessCreateSystem(BusinessConfig[] configs)
        {
            _configs = configs;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var configPool = world.GetPool<BusinessConfigComponent>();
            var activeBusinessPool = world.GetPool<ActiveBusinessComponent>();

            var sharedData = systems.GetShared<SharedData>();

            foreach (var config in _configs)
            {
                var entity = world.NewEntity();

                ref BusinessConfigComponent configComponent = ref configPool.Add(entity);
                configComponent.config = config;

                if (sharedData.HasSavedData)
                {
                    TryLoadBusinessData(activeBusinessPool, sharedData, config, entity);
                }
                else if (config.ActiveOnStart)
                {
                    ref var activeBusiness = ref activeBusinessPool.Add(entity);
                }
            }
        }

        private void TryLoadBusinessData(EcsPool<ActiveBusinessComponent> activeBusinessPool, SharedData sharedData, BusinessConfig config, int entity)
        {
            foreach (var business in sharedData.SavedData.businessDatas)
            {
                if (!business.businessName.Equals(config.BusinesName))
                    continue;

                ref var activeBusiness = ref activeBusinessPool.Add(entity);
                activeBusiness.businessLevel = business.level;
                activeBusiness.profitProgress = business.progress;
                activeBusiness.purchasedUpgrades = business.purchasedUpgrades;
            }
        }
    }
}