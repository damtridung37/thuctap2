using System.Collections.Generic;
using UnityEngine;

namespace D
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Room : MonoBehaviour
    {
        private List<BoxCollider2D> roomBorder;
        public int doorCount;
        [Header("Portal")]
        [SerializeField]
        private Portal portal;

        [Header("Chest")]
        [SerializeField] private GameObject Chest;

        [Header("Shop")]
        [SerializeField] private Shop ShopArea;

        [Header("SpawnPoints")]
        [SerializeField] private int waveCount;
        [SerializeField] private float delayBetweenWaves;
        [SerializeField] private Transform spawnPoints;
        public Transform playerSpawnPoint;
        public Transform bossSpawnPoint;
        private bool isCleared = false;

        public enum RoomType
        {
            Entrance,
            Shop,
            Arena,
            Boss,
            Treasure,
            Portal
        }

        private void Awake()
        {
            var temp = GetComponentsInChildren<BoxCollider2D>();
            var thisCollider = GetComponent<BoxCollider2D>();
            roomBorder = new List<BoxCollider2D>();
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] != thisCollider)
                {
                    roomBorder.Add(temp[i]);
                }
            }
        }

        bool isFirstTime = true;

        private void OnTriggerEnter2D(Collider2D other)
        {
            GameManager.Instance.currentRoom = this;


            if (other.CompareTag("Player"))
            {
                if (roomType == RoomType.Arena)
                {
                    if (!isFirstTime || isCleared) return;
                    isFirstTime = false;

                    SpawnEnemy();

                    isCleared = false;

                    UnlockRoom();
                    GlobalEvent<bool>.Subscribe("Clear_Enemy", UnlockRoom);
                }
                else if (roomType == RoomType.Boss)
                {
                    if (!isFirstTime || isCleared) return;
                    isFirstTime = false;
                    var boss = PrefabManager.Instance.SpawnBoss(bossSpawnPoint.position, D.GameManager.Instance.playerData.CurrentFloor);
                    Debug.Log("Spawning Boss");
                    boss.gameObject.SetActive(true);
                    UnlockRoom();
                }
            }
        }

        public void SpawnEnemy()
        {
            foreach (var transform in spawnPoints.transform)
            {
                Debug.Log("Spawning Enemy");
                var enemy = PrefabManager.Instance.GetRandomEnemy();
                enemy.transform.position = ((Transform)transform).position;
                enemy.InitStats();
            }
        }

        private void UnlockRoom(bool a = false)
        {
            /*Debug.Log("Room: " + this.name);
            Debug.Log("Unlocking Room" + a);*/
            if (a)
            {
                GlobalEvent<(int, bool)>.Trigger("On_PlayerGoldChanged", (GameManager.Instance.staticConfig.GOLD_PER_WAVE, true));
                GlobalEvent<bool>.Unsubscribe("Clear_Enemy", UnlockRoom);
                isCleared = true;
                roomBorder[0].transform.parent.gameObject.SetActive(false);
                PrefabManager.Instance.ClearEnemy();
            }
            else
                foreach (var collider in roomBorder)
                {
                    collider.isTrigger = a;
                }
        }

        private RoomType roomType;

        public void Init(RoomType roomType)
        {
            this.roomType = roomType;
            switch (roomType)
            {
                case RoomType.Entrance:
                    portal.gameObject.SetActive(true);
                    portal.Init();
                    break;
                case RoomType.Shop:
                    ShopArea.gameObject.SetActive(true);
                    break;
                case RoomType.Arena:

                    break;
                case RoomType.Boss:
                    break;
                case RoomType.Treasure:
                    Chest.SetActive(true);
                    break;
                case RoomType.Portal:
                    portal.gameObject.SetActive(true);
                    portal.Init(false);
                    break;
            }
        }
    }

}
