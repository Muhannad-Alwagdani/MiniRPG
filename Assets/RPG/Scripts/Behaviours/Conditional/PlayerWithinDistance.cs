using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace RPG.Scripts.Behaviours.Conditional
{
    public class PlayerWithinDistance : BehaviorDesigner.Runtime.Tasks.Conditional
    {
        private Transform _playerTransform;
        [SerializeField] private float distance = 5f;
        public override void OnAwake()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public override TaskStatus OnUpdate()
        {
            if(_playerTransform != null)
            {
                if (Vector3.Distance(transform.position, _playerTransform.position) <= distance)
                    return TaskStatus.Success;
                else
                    return TaskStatus.Failure;
            }
            else
                return TaskStatus.Failure;
        }
    }
}