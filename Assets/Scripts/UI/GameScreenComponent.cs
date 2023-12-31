using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TestAsssignment.Systems.GameUISystem;

namespace TestAsssignment.UI
{
    public class GameScreenComponent : MonoBehaviour
    {
        public TMPro.TMP_Text balanceValueText;

        public Button quitButton;

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

            quitButton.onClick.AddListener(Application.Quit);
        }

        public void UpdateInfo()
        {
            for (int i = 0; i < businessesListItems.Length; i++)
            {
                businessesListItems[i].UpdateInfo();
            }
        }

        public void SetBalanceValue(double value)
        {
            balanceValueText.SetText($"${value}");
        }
    }
}