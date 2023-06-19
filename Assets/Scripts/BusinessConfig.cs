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
        private double baseCost;
        [SerializeField]
        private double baseProfit;

        [Serializable]
        public struct BusinessImprovement
        {
            public double improvementCost;
            public double profitMultiplier;
        }

        [SerializeField]
        private BusinessImprovement[] improvements;

        [SerializeField]
        private bool activeOnStart;

        public string BusinesName => businessName;
        public int ProfitDelay => profitDelay;
        public double BaseCost => baseCost;
        public double BaseProfit => baseProfit;

        public BusinessImprovement[] Improvements => improvements;

        public bool ActiveOnStart => activeOnStart;

    }
}