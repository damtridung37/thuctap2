using System;
using UnityEngine;

namespace D
{
    public class AgisBullet : MonoBehaviour, IPoolable<AgisBullet>
    {
        public Vector2 direction;

        private Action<AgisBullet> returnAction;

        public Action onHitPlayer;

        public bool isShoot = false;
        private float timeToDisable = 10f;


        private void OnEnable()
        {
            timeToDisable = 10f;
        }

        private void ForceDisable()
        {
            gameObject.SetActive(false);
        }

        public void Initialize(Action<AgisBullet> returnAction)
        {
            this.returnAction = returnAction;
        }

        public void ReturnToPool()
        {
            isShoot = false;
            this.returnAction(this);
        }

        private void Update()
        {
            timeToDisable -= Time.deltaTime;
            if (timeToDisable <= 0)
            {
                ReturnToPool();
            }
            if (!isShoot) return;
            transform.position += (Vector3)direction * 30 * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                onHitPlayer?.Invoke();
                ReturnToPool();
            }
        }
    }
}
