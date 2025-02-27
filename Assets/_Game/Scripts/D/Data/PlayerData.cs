using System;
using System.Collections.Generic;
using UnityEngine;

namespace D
{
    [Serializable]
    public class PlayerData
    {
        // current progress
        [SerializeField] private int currentGold = 0;
        [SerializeField] private int currentLevel = 1;
        [SerializeField] private int currentExp = 0;
        [SerializeField] private int currentStatPoints = 0;
        [SerializeField] private int currentFloor = 1;
        [SerializeField] private float currentHealth = -1;

        public StatDictionary playerBonusStats = new StatDictionary();

        public int CurrentGold
        {
            get => currentGold;
            set => currentGold = value;
        }

        public int CurrentLevel
        {
            get => currentLevel;
            set => currentLevel = value;
        }

        public int CurrentExp
        {
            get => currentExp;
            set => currentExp = value;
        }

        public int CurrentStatPoints
        {
            get => currentStatPoints;
            set => currentStatPoints = value;
        }

        public int CurrentFloor
        {
            get => currentFloor;
            set
            {
                currentFloor = value;
                if (currentFloor > highestFloor)
                {
                    highestFloor = currentFloor;
                }
            }
        }

        public IDictionary<StatType, float> PlayerBonusStats
        {
            get => playerBonusStats;
            set => playerBonusStats.CopyFrom(value);
        }

        public float CurrentHealth
        {
            get => currentHealth;
            set => currentHealth = value;
        }


        // Highest progress
        [SerializeField] private int highestFloor = 1;

        public int HighestFloor
        {
            get => highestFloor;
            set => highestFloor = value;
        }

        // Other
        [SerializeField] private string uuid;
        public string Uuid => uuid;

        public PlayerData()
        {
            uuid = SystemInfo.deviceUniqueIdentifier;
            foreach (StatType statType in Enum.GetValues(typeof(StatType)))
            {
                PlayerBonusStats.Add(statType, 0);
            }
        }

        public void Reset()
        {
            currentGold = 0;
            currentLevel = 1;
            currentExp = 0;
            currentFloor = 1;
            currentHealth = -1;
        }
    }

    [Serializable]
    public class StatBonusDictionary : SerializableDictionary<StatType, int>
    {
    }
}
