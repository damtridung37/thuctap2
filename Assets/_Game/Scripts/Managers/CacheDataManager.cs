using UnityEngine;

public class CacheDataManager : MonoBehaviour
{
    public static CacheDataManager instance;
    
    public Player player;
    public ExpPickUp ExpPickUpPrefab;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
}
