using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Scripts
{
   public class InputReceiver : MonoBehaviour
   {
      [SerializeField] private GameEvent meleeAttackEvent;
      [SerializeField] private GameEvent specialAttackEvent;
      [SerializeField] private Vector2Reference movementVectorReference;

      public void Move2D(InputAction.CallbackContext ctx)
      {
         movementVectorReference.Value = ctx.ReadValue<Vector2>();
      }

      public void Attack(InputAction.CallbackContext ctx)
      {
         if (ctx.started)
         {
            meleeAttackEvent.Raise();
         }
      }
      public void SpecialAttack(InputAction.CallbackContext ctx)
      {
         if (ctx.started)
         {
            specialAttackEvent.Raise();
         }
      }
   }
}
