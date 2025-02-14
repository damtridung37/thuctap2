using System.Collections;
using UnityEngine;
using WFC;

namespace D
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private WFCGrid wfcGrid;
        public Room currentRoom;
        private int currentFloor = 1;
        
        void Start()
        {
            if(PlayerPrefs.HasKey("CurrentFloor"))
                currentFloor = PlayerPrefs.GetInt("CurrentFloor");
            else
                PlayerPrefs.SetInt("CurrentFloor", currentFloor);
            InitEvent();
            GetMap();
        }
        
        private void InitEvent()
        {
            GlobalEvent<bool>.Subscribe("OnPlayerDead", (a)
                =>
            {
                GetMap(-1,a);
                Player.Instance.InitStats();
                Player.Instance.IsDead = false;
            } );
        }

        public void GetMap(int floor = -1,bool isGameover = false)
        {
            if(floor == -1)
                floor = currentFloor;
            StartCoroutine(LoadFloor(floor,isGameover));
        }
        
        public void GetNextMap()
        {
            GetMap(currentFloor + 1);
        }
        
        IEnumerator LoadFloor(int floor, bool isGameover = false)
        {
            currentFloor = floor;
            PlayerPrefs.SetInt("CurrentFloor", currentFloor);
            UIManager.Instance.LoadingScreen.Open(isGameover);
            float time = Time.time;
            
            yield return wfcGrid.StartCoroutine(nameof(wfcGrid.Init),3);
            if(Time.time - time < 2)
                yield return new WaitForSeconds(2 - (Time.time - time));
            
            UIManager.Instance.LoadingScreen.Close();
        }
    }
}
