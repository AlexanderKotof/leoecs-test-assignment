using Leopotam.EcsLite;
using TestAsssignment.Configs;

namespace TestAsssignment.Components
{
    public struct ActiveBusinessComponent : IEcsAutoReset<ActiveBusinessComponent>
    {
        public int businessLevel;

        public float profitProgress;

        public bool[] purchasedUpgrades;

        public void AutoReset(ref ActiveBusinessComponent c)
        {
            c.businessLevel = 1;
            c.profitProgress = 0;
            c.purchasedUpgrades = new bool[BusinessConfig.upgradesCount];
        }
    }
}