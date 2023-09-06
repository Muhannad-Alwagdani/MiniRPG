using RPG.Scripts.Interfaces;
using UnityEngine;

namespace RPG.Scripts
{
    public class Prop : MonoBehaviour, IDamageable
    {
        [SerializeField] private GameObject dropPrefab;
        public void ApplyDamage(float value = 1, Vector3 hitDirection = default)
        {
            if(dropPrefab)
                Instantiate(dropPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}