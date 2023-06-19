using TestAsssignment.Configs;

namespace TestAsssignment.Utils
{
    public static class GameMath
    {
        public static double CalculateBusinessProfit(BusinessConfig config, int businessLevel, bool[] purchasedUpgrades)
        {
            double value = config.BaseProfit * businessLevel;
            double multiplier = 1;

            for (int i = 0; i < purchasedUpgrades.Length; i++)
            {
                if (purchasedUpgrades[i])
                    multiplier += config.Upgrades[i].profitMultiplier;
            }

            return value * multiplier;
        }

        public static double GetBusinessLvlUpPrice(BusinessConfig config, int currentLevel)
        {
            return (currentLevel + 1) * config.BasePrice;
        }
    }
}