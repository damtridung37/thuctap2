using System;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;

namespace D
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private LoadingScreen loadingScreen;
        [SerializeField] private ShopUI shopUI;

        public ShopUI ShopUI => shopUI;
        public LoadingScreen LoadingScreen => loadingScreen;

        [SerializeField] private TMP_Text floorText;

        void Awake()
        {
            GlobalEvent<int>.Subscribe("On_PlayerFloorChanged", UpdateFloorText);
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

        private void DisableFloorText()
        {
            floorText.gameObject.SetActive(false);
        }
    }
}
