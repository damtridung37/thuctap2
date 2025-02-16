using UnityEngine;
namespace D
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class Shop : MonoBehaviour
    {
        private PolygonCollider2D shopBorder;

        private void Awake()
        {
            shopBorder = GetComponent<PolygonCollider2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                int seed = this.gameObject.GetInstanceID();
                UIManager.Instance.ShopUI.Init(seed);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player exited shop");
                UIManager.Instance.ShopUI.Close();
            }
        }
    }
}
