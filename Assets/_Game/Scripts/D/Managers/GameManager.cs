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
                    UIManager.Instance.UpdateGoldText(playerData.CurrentGold);
                });
            GlobalEvent<(int, bool)>.Subscribe("On_PlayerStatPointChanged",
                (statPoint) =>
                {
                    playerData.CurrentStatPoints = statPoint.Item2 ? playerData.CurrentStatPoints + statPoint.Item1 : playerData.CurrentStatPoints - statPoint.Item1;
                });

            GlobalEvent<HealthData>.Subscribe("PlayerHealthChanged", (data) =>
            {
                playerData.CurrentHealth = data.currentHealth;
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
            Player.Instance.ColliderEnable(false);
            StartCoroutine(LoadFloor(floor, isGameover));
        }

        public void GetNextMap()
        {
            //Player.Instance.InitStats();
            GlobalEvent<(int, bool)>.Trigger("On_PlayerStatPointChanged", (GameManager.Instance.staticConfig.STAT_POINTS_PER_FLOOR, true));
            GetMap(playerData.CurrentFloor + 1);
        }

        IEnumerator LoadFloor(int floor, bool isGameover = false)
        {
            playerData.CurrentFloor = floor;
            if (isGameover)
                playerData.Reset();
            FirebaseManager.Instance.RealtimeDatabase.SaveData(playerData);
            UIManager.Instance.LoadingScreen.Open(isGameover);
            float time = Time.time;

            yield return wfcGrid.StartCoroutine(nameof(wfcGrid.Init), 3);
            if (Time.time - time < 2)
                yield return new WaitForSeconds(2 - (Time.time - time));

            GlobalEvent<int>.Trigger("On_PlayerFloorChanged", floor);

            UIManager.Instance.LoadingScreen.Close();

            yield return new WaitForSeconds(2f);

            UIManager.Instance.StatUpgradeUI.gameObject.SetActive(true);

            Player.Instance.ColliderEnable(true);
        }

        public Vector3 MapCenter()
        {
            return wfcGrid.MapCenter;
        }

        public Vector2Int GridSize()
        {
            return wfcGrid.MapSize;
        }
    }
}
