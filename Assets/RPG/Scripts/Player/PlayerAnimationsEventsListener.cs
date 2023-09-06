using UnityEngine;

namespace RPG.Scripts.Player
{
    public class PlayerAnimationsEventsListener : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        public void PerformMeleeAttack()
        {
            playerController.PerformMeleeAttackCollision();
        }
    }
}