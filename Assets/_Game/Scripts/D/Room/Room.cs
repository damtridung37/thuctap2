using UnityEngine;

namespace D
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Room : MonoBehaviour
    {
        private BoxCollider2D roomBorder;
        
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
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log($"Player Exited Room: {name}");
            }
        }
    }
    
}
