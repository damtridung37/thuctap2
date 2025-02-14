using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace D
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Room : MonoBehaviour
    {
        private BoxCollider2D roomBorder;
        [Header("Portal")]
        [SerializeField]
        private Portal portal;
        
        [Header("Chest")]
        [SerializeField] private GameObject Chest;

        [Header("Shop")]
        [SerializeField] private GameObject ShopArea;
        
        [Header("SpawnPoints")]
        [SerializeField] private int waveCount;
        [SerializeField] private float delayBetweenWaves;
        [SerializeField] private Transform spawnPoints;
        private List<Enemy> activeEnemy;
        
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
            roomBorder = GetComponent<BoxCollider2D>();
            roomBorder.isTrigger = true;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log($"Player Entered Room: {name}");
                if(roomType != RoomType.Arena) return;
                activeEnemy = new List<Enemy>();
                foreach (var transform in spawnPoints.transform)
                {
                    Debug.Log("Spawning Enemy");
                    var enemy = PrefabManager.Instance.GetRandomEnemy();
                    enemy.transform.position = ((Transform) transform).position;
                    activeEnemy.Add(enemy);
                }
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log($"Player Exited Room: {name}");
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
                    ShopArea.SetActive(true);
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
                    portal.Init();
                    break;
            }
        }
    }
    
}
