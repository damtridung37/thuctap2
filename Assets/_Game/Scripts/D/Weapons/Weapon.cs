using UnityEngine;

namespace D
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] protected Ammo arrowPrefab;
        [SerializeField] protected float ammoSpeed;
        protected Character character;
        [SerializeField] protected Transform firePoint;

        protected ObjectPool<Ammo> ammoPool;
        protected Transform ammoHolder;

        protected virtual void Awake()
        {
            ammoHolder = new GameObject(arrowPrefab.name).transform;
            ammoPool = new ObjectPool<Ammo>(arrowPrefab, ammoHolder, 10);
        }

        public virtual void Attack(Character character)
        {
            Debug.Log("Weapon Attack");
        }
    }
}
