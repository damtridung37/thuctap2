using UnityEngine;
using UnityEngine.SceneManagement;

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
                if (GameManager.Instance.playerData.CurrentFloor % 10 == 0) // boss floor
                {
                    GameManager.Instance.playerData.CurrentFloor += 1;
                    FirebaseManager.Instance.RealtimeDatabase.SaveData(GameManager.Instance.playerData);
                    SceneManager.LoadScene("Win");
                }
                else
                    GameManager.Instance.GetNextMap();
            }
        }
    }
}
