using System.Collections;
using UnityEngine;
using WFC;

namespace D
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private WFCGrid wfcGrid;
        public Room currentRoom;
        public PlayerData playerData;
        public StaticConfig staticConfig;

        IEnumerator Start()
        {
            Debug.Log("Waiting for Firebase to initialize...");
            while (!FirebaseManager.Instance.FirebaseInitialized)
            {
                yield return null;
            }
            playerData = FirebaseManager.Instance.PlayerData;
            InitEvent();
            GetMap(playerData.CurrentFloor);
            Player.Instance.InitStats();
        }

        private void InitEvent()
        {
            GlobalEvent<bool>.Subscribe("OnPlayerDead", (a)
                =>
            {
                GetMap(1, a);
                Player.Instance.InitStats();
                Player.Instance.IsDead = false;
            });
            GlobalEvent<ExpData>.Subscribe("PlayerExpChanged",
                (expData) =>
                {
                    playerData.CurrentExp = expData.currentExp;
                    playerData.CurrentLevel = expData.currentLevel;
                });
            GlobalEvent<(int, bool)>.Subscribe("On_PlayerGoldChanged",
                (data) =>
                {
                    var (gold, isAdd) = data;
                    playerData.CurrentGold = isAdd ? playerData.CurrentGold + gold : playerData.CurrentGold - gold;
                });
            GlobalEvent<int>.Subscribe("On_PlayerStatPointChanged",
                (statPoint) =>
                {
                    playerData.CurrentStatPoints = statPoint;
                });
        }

        private void OnApplicationQuit()
        {
            GlobalEvent<bool>.ClearAllEvents();
            GlobalEvent<ExpData>.ClearEvent("PlayerExpChanged");
            GlobalEvent<(int, bool)>.ClearEvent("On_PlayerGoldChanged");
            GlobalEvent<int>.ClearAllEvents();
            GlobalEvent<HealthData>.ClearEvent("PlayerHealthChanged");
            GlobalEvent<(StatType, float, bool)>.ClearAllEvents();
        }

        public void GetMap(int floor = 1, bool isGameover = false)
        {
            StartCoroutine(LoadFloor(floor, isGameover));
        }

        public void GetNextMap()
        {
            GetMap(playerData.CurrentFloor + 1);
        }

        IEnumerator LoadFloor(int floor, bool isGameover = false)
        {
            playerData.CurrentFloor = floor;
            UIManager.Instance.LoadingScreen.Open(isGameover);
            float time = Time.time;

            yield return wfcGrid.StartCoroutine(nameof(wfcGrid.Init), 3);
            if (Time.time - time < 2)
                yield return new WaitForSeconds(2 - (Time.time - time));

            GlobalEvent<int>.Trigger("On_PlayerFloorChanged", floor);
            SaveManager<PlayerData>.Save(playerData);
            UIManager.Instance.LoadingScreen.Close();
        }
    }
}
