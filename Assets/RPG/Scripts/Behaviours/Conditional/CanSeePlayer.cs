using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace RPG.Scripts.Behaviours.Conditional
{
    public class CanSeePlayer : BehaviorDesigner.Runtime.Tasks.Conditional
    {
        private Transform _playerTransform;
        public override void OnAwake()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public override TaskStatus OnUpdate()
        {
            if(Physics2D.Raycast(transform.position, _playerTransform.position - transform.position, 100f, LayerMask.GetMask("Player")))
                return TaskStatus.Success;
            else
                return TaskStatus.Failure;

            }
    }
}