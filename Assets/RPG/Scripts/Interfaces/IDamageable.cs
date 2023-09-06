using UnityEngine;

namespace RPG.Scripts.Interfaces
{
    public interface IDamageable
    {
        public void ApplyDamage(float value = 1, Vector3 hitDirection = default){}
    }
}