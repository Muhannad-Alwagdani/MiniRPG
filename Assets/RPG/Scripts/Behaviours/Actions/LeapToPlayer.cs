using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Scripts.Behaviours.Actions
{
    public class LeapToPlayer : Action
    {
        private Transform _playerTransform;
        private Rigidbody2D _rigidbody2D;
        private NavMeshAgent _navMeshAgent;
        
        private Vector3 _leapTargetPosition;
        public override void OnAwake()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public override void OnStart()
        {
            _leapTargetPosition = _playerTransform.position;
        }

        public override TaskStatus OnUpdate()
        {
            float distance = Vector3.Distance(transform.position, _playerTransform.position);
            if (distance <= 0.1f)
            {
                return TaskStatus.Success;
            }else if (distance > _navMeshAgent.stoppingDistance)
            {
                return TaskStatus.Failure;
            }
            else
            {
                _rigidbody2D.MovePosition(Vector3.MoveTowards(transform.position, _leapTargetPosition, 10f * Time.deltaTime));
                return TaskStatus.Running;
            }
        }
    }
}