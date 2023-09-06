using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Scripts.Behaviours.Actions
{
    public class MoveAroundThePlayer : Action
    {
        [SerializeField] private float randomRadius = 5f;
        private Transform _playerTransform;
        private NavMeshAgent _navMeshAgent;
        public override void OnAwake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public override void OnStart()
        {
            
            SetRandomDestination();
        }

        private void SetRandomDestination()
        {
            //Set destination to a random point around the player withing navmesh
            Vector3 randomDirection = Random.insideUnitSphere * randomRadius + new Vector3(2f, 2f, 0f);
            randomDirection += _playerTransform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, randomRadius, 1);
            Vector3 finalPosition = hit.position;
            _navMeshAgent.SetDestination(finalPosition);
        }

        public override TaskStatus OnUpdate()
        {
            if(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                return TaskStatus.Success;
            else
            {
                SetRandomDestination();
                return TaskStatus.Running;
            }
        }
    }
}