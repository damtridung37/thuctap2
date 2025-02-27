using UnityEngine;

namespace D
{
    public class Portal : MonoBehaviour
    {
        private bool isEntrance;
        public void Init(bool isEntrance = true)
        {
            this.isEntrance = isEntrance;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isEntrance && other.TryGetComponent(out Character character))
            {
                GameManager.Instance.GetNextMap();
            }
        }
    }
}
