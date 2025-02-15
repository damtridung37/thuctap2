using System;
using System.Collections.Generic;
using UnityEngine;

namespace D
{
    [Serializable]
    public class PlayerData
    {
        // current progress
        private int currentGold = 0;
        private int currentLevel = 1;
        private int currentExp = 0;
        private int currentStatPoints = 0;
        private int currentFloor = 1;
        
        private Dictionary<StatType,int> playerStats = new Dictionary<StatType, int>();

        public int CurrentGold
        {
            get => currentGold;
            set
            {
                currentGold = value;
            }
        }
        
        public int CurrentLevel
        {
            get => currentLevel;
            set
            {
                currentLevel = value;
            }
        }
        
        public int CurrentExp
        {
            get => currentExp;
            set
            {
                currentExp = value;
            }
        }
        
        public int CurrentStatPoints
        {
            get => currentStatPoints;
            set
            {
                currentStatPoints = value;
            }
        }
        
        public Dictionary<StatType, int> PlayerStats
        {
            get => playerStats;
            set
            {
                playerStats = value;
            }
        }
        
        public int CurrentFloor
        {
            get => currentFloor;
            set
            {
                currentFloor = value;
                if(currentFloor > highestFloor)
                    highestFloor = currentFloor;
            }
        }
        
        // Highest progress
        private int highestFloor;
        
        public int HighestFloor
        {
            get => highestFloor;
            set
            {
                highestFloor = value;
            }
        }
    }
}
