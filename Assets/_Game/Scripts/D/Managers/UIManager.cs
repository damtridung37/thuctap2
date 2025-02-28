using System;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace D
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private LoadingScreen loadingScreen;
        [SerializeField] private ShopUI shopUI;
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private StatUpgradeUI statUpgradeUI;

        [SerializeField] private TMP_Text bossName;
        [SerializeField] private TMP_Text bossCurrentHP;
        [SerializeField] private Image bossHP;
        [SerializeField] private Canvas bossCanvas;
        public StatUpgradeUI StatUpgradeUI => statUpgradeUI;

        public ShopUI ShopUI => shopUI;
        public LoadingScreen LoadingScreen => loadingScreen;

        [SerializeField] private TMP_Text floorText;

        void OnEnable()
        {
            GlobalEvent<int>.Subscribe("On_PlayerFloorChanged", UpdateFloorText);
        }

        void OnDisable()
        {
            GlobalEvent<int>.Unsubscribe("On_PlayerFloorChanged", UpdateFloorText);
        }

        public void UpdateFloorText(int floor)
        {
            floorText.text = String.Empty;
            floorText.gameObject.SetActive(true);
            LMotion.String.Create128Bytes("", $"Floor {floor}", 1f)
                .WithScrambleChars(ScrambleMode.Lowercase)
                .WithOnComplete(() => Invoke(nameof(DisableFloorText), 1f))
                .BindToText(floorText);
        }

        public void UpdateGoldText(int gold)
        {
            goldText.text = gold.ToString();
        }

        private void DisableFloorText()
        {
            floorText.gameObject.SetActive(false);
        }

        public void InitBossUI(string name, HealthData data)
        {
            bossName.text = name;
            bossHP.fillAmount = 1f;
            bossCurrentHP.text = $"{data.maxHealth} / {data.maxHealth}";
            bossCanvas.gameObject.SetActive(true);
            GlobalEvent<HealthData>.Subscribe("BossHealthChanged", UpdateBossData);
        }

        public void DisableBossUI()
        {
            bossCanvas.gameObject.SetActive(false);
            GlobalEvent<HealthData>.Unsubscribe("BossHealthChanged", UpdateBossData);
        }

        public void UpdateBossData(HealthData data)
        {
            bossHP.fillAmount = data.currentHealth / data.maxHealth;
            bossCurrentHP.text = $"{Mathf.FloorToInt(data.currentHealth)} / {data.maxHealth}";
        }
    }
}
