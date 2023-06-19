using Leopotam.EcsLite;
using System.Collections.Generic;
using TestAsssignment.Components;
using UnityEngine;
using static TestAsssignment.Systems.GameUISystem;

namespace TestAsssignment.UI
{
    public class GameScreenComponent : MonoBehaviour
    {
        public TMPro.TMP_Text balanceValueText;

        public BusinessItemComponent businessesListItemPrefab;

        public Transform businessesListTransform;

        private BusinessItemComponent[] businessesListItems;

        public void SetInfo(List<BusinessData> datas)
        {
            businessesListItems = new BusinessItemComponent[datas.Count];
            for (int i = 0; i < datas.Count; i++)
            {
                businessesListItems[i] = GameObject.Instantiate(businessesListItemPrefab, businessesListTransform);
                businessesListItems[i].SetInfo(datas[i]);
            }
        }

        public void SetBalanceValue(double value)
        {
            balanceValueText.SetText($"${value}");
        }

        public void UpdateScreen(EcsPool<ActiveBusinessComponent> _activeBuisnessesPool)
        {
            foreach (var item in businessesListItems)
            {
                if (_activeBuisnessesPool.Has(item.Data.entity))
                {
                    item.UpdateItem(_activeBuisnessesPool.Get(item.Data.entity).profitProgress);
                }
            }
        }
    }
}