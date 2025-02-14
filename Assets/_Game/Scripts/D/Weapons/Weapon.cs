using UnityEngine;

namespace D
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] protected Transform firePoint;
        public virtual void Attack(Character character)
        {
            Debug.Log("Weapon Attack");
        }
    }
}
