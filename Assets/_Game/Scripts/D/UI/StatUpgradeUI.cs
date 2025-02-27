using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace D
{
    public class StatUpgradeUI : MonoBehaviour
    {
        [SerializeField] private Button backBtn;

        [Header("Stat upgrade buttons")]
        [SerializeField] private Button damageUpgradeBtn;
        [SerializeField] private Button healthUpgradeBtn;
        [SerializeField] private Button armorUpgradeBtn;
        [SerializeField] private Button hitUpgradeBtn;
        [SerializeField] private Button critUpgradeBtn;

        [Header("Stat upgrade texts")]
        [SerializeField] private TMP_Text damageUpgradeText;
        [SerializeField] private TMP_Text healthUpgradeText;
        [SerializeField] private TMP_Text armorUpgradeText;
        [SerializeField] private TMP_Text hitUpgradeText;
        [SerializeField] private TMP_Text critUpgradeText;
        [SerializeField] private TMP_Text statPointText;

        private PlayerData _playerData;

        private void OnEnable()
        {
            if (_playerData != null)
            {
                DisplayText();
            }
        }

        private void Start()
        {
            _playerData = GameManager.Instance.playerData;
            DisplayText();
            Init();
        }

        private void DisplayText()
        {
            damageUpgradeText.text = "Point: " + _playerData.PlayerBonusStats[StatType.Damage].ToString();
            healthUpgradeText.text = "Point: " + _playerData.PlayerBonusStats[StatType.Health].ToString();
            armorUpgradeText.text = "Point: " + _playerData.PlayerBonusStats[StatType.Armor].ToString();
            hitUpgradeText.text = "Point: " + _playerData.PlayerBonusStats[StatType.CritChance].ToString();
            critUpgradeText.text = "Point: " + _playerData.PlayerBonusStats[StatType.CritDamage].ToString();
            statPointText.text = "Stat Points Remaining: " + _playerData.CurrentStatPoints.ToString();
        }


        private void Init()
        {
            damageUpgradeBtn.onClick.AddListener(() => OnClickUpgrade(StatType.Damage));
            healthUpgradeBtn.onClick.AddListener(() => OnClickUpgrade(StatType.Health));
            armorUpgradeBtn.onClick.AddListener(() => OnClickUpgrade(StatType.Armor));
            hitUpgradeBtn.onClick.AddListener(() => OnClickUpgrade(StatType.CritChance));
            critUpgradeBtn.onClick.AddListener(() => OnClickUpgrade(StatType.CritDamage));
            backBtn.onClick.AddListener(() => gameObject.SetActive(false));
        }

        private void OnClickUpgrade(StatType statType)
        {
            if (_playerData.CurrentStatPoints <= 0)
            {
                for (int i = 0; i < statPointText.textInfo.characterCount; i++)
                {
                    LMotion.Create(Color.white, Color.red, 1f)
                        .WithDelay(i * 0.1f)
                        .WithEase(Ease.OutQuad)
                        .BindToTMPCharColor(statPointText, i);

                    LMotion.Punch.Create(Vector3.zero, Vector3.up * 30f, 1f)
                        .WithDelay(i * 0.1f)
                        .WithEase(Ease.OutQuad)
                        .WithOnComplete(() =>
                        {
                            LMotion.Create(Color.red, Color.white, 1f)
                                .WithDelay(i * 0.1f)
                                .WithEase(Ease.OutQuad)
                                .BindToTMPCharColor(statPointText, i);
                        })
                        .BindToTMPCharPosition(statPointText, i);
                }
                return;
            }
            _playerData.CurrentStatPoints--;
            _playerData.PlayerBonusStats[statType]++;
            Player.Instance.ReCalculateStats();
            statPointText.text = "Stat Points Remaining: " + _playerData.CurrentStatPoints.ToString();
            switch (statType)
            {
                case StatType.Damage:
                    damageUpgradeText.text = "Point: " + _playerData.PlayerBonusStats[statType].ToString();
                    break;
                case StatType.Health:
                    healthUpgradeText.text = "Point: " + _playerData.PlayerBonusStats[statType].ToString();
                    break;
                case StatType.Armor:
                    armorUpgradeText.text = "Point: " + _playerData.PlayerBonusStats[statType].ToString();
                    break;
                case StatType.CritChance:
                    hitUpgradeText.text = "Point: " + _playerData.PlayerBonusStats[statType].ToString();
                    break;
                case StatType.CritDamage:
                    critUpgradeText.text = "Point: " + _playerData.PlayerBonusStats[statType].ToString();
                    break;
            }
        }
    }
}
