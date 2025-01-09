using UnityEngine;

public class CacheDataManager : Singleton<CacheDataManager>
{
    public Player player;
    public ExpPickUp ExpPickUpPrefab;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
}
