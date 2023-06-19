using TestAsssignment.Configs;

namespace TestAsssignment.Components
{
    public struct BusinessConfigComponent
    {
        public BusinessConfig config;
    }

    public struct ActiveBusinessComponent
    {
        public int businessLevel;

        public float profitProgress;

        public bool[] purchasedUpgrades;
    }

    public interface IChangeMoney
    {

    }

    public struct AddMoney : IChangeMoney
    {
        public double value;
    }

    public struct SpendMoney : IChangeMoney
    {
        public double value;
    }

    public struct IncreaseBusinessLevel
    {
        public int entity;
    }

    public struct BuyBusinessUpgrade
    {
        public int entity;
        public int index;
    }
}