using System;
using UnityEngine;

namespace TestAsssignment.Configs
{
    [CreateAssetMenu(menuName = "Configs/Business")]
    public class BusinessConfig : ScriptableObject
    {
        [SerializeField]
        private string businessName;
        [SerializeField]
        private int profitDelay;
        [SerializeField]
        private double basePrice;
        [SerializeField]
        private double baseProfit;

        [Serializable]
        public struct BusinessUpgrade
        {
            public string name;
            public double upgradePrice;
            public double profitMultiplier;
        }

        [SerializeField]
        private BusinessUpgrade[] upgrades = new BusinessUpgrade[upgradesCount];

        [SerializeField]
        private bool activeOnStart;

        public string BusinesName => businessName;
        public int ProfitDelay => profitDelay;
        public double BasePrice => basePrice;
        public double BaseProfit => baseProfit;

        public BusinessUpgrade[] Upgrades => upgrades;

        public bool ActiveOnStart => activeOnStart;

        public const int upgradesCount = 2;

    }
}