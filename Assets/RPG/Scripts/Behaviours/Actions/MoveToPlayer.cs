using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Scripts.Behaviours.Actions
{
    public class MoveToPlayer : Action
    {
        private Transform _playerTransform;
        private NavMeshAgent _navMeshAgent;
        public override void OnAwake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public override void OnStart()
        {
            _navMeshAgent.SetDestination(_playerTransform.position);
        }

        public override TaskStatus OnUpdate()
        {
            if(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                return TaskStatus.Success;
            else
            {
                _navMeshAgent.SetDestination(_playerTransform.position);
                return TaskStatus.Running;
            }
        }
    }
}