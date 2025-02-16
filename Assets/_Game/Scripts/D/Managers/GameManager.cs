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
        
        void Awake()
        {
            //SaveManager<PlayerData>.Delete();
            playerData = SaveManager<PlayerData>.Load(true);
        }
        void Start()
        {
            InitEvent();
            GetMap();
        }
        
        private void InitEvent()
        {
            GlobalEvent<bool>.Subscribe("OnPlayerDead", (a)
                =>
            {
                GetMap(1,a);
                Player.Instance.InitStats();
                Player.Instance.IsDead = false;
            } );
        }

        public void GetMap(int floor = 1,bool isGameover = false)
        {
            StartCoroutine(LoadFloor(floor,isGameover));
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
            
            yield return wfcGrid.StartCoroutine(nameof(wfcGrid.Init),3);
            if(Time.time - time < 2)
                yield return new WaitForSeconds(2 - (Time.time - time));
            
            GlobalEvent<int>.Trigger("On_PlayerFloorChanged", floor);
            SaveManager<PlayerData>.Save(playerData);
            UIManager.Instance.LoadingScreen.Close();
        }
    }
}
