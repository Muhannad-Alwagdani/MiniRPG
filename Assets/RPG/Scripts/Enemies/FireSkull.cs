using System;
using BehaviorDesigner.Runtime;
using RPG.Scripts.Interfaces;
using RPG.Scripts.Misc;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Scripts.Enemies
{
    public class FireSkull : MonoBehaviour, IDamageable, IHookable
    {
        [SerializeField] private FloatReference fireSkullInitialHealthReference;
        [SerializeField] private float fireBallProjectileSpeed;


        private Transform _playerTransform;
        private Animator _animator;
        private Collider2D _collider2D;
        private NavMeshAgent _navMeshAgent;
        private BehaviorTree _behaviorTree;
        
        
        private float _fireSkullHealth;

        public void ApplyDamage(float value = 1, Vector3 hitDirection = default)
        {
            _fireSkullHealth -= value;
            if (_fireSkullHealth <= 0)
            {
                Death();
            }
        }
        public void Hook()
        {
            //not implemented
        }
        public void ShootFireBall()
        {
            GameObject fireBall = FireBallManager.Instance.GetFireBall();
            fireBall.transform.position = transform.position;
            fireBall.SetActive(true);
            fireBall.GetComponent<Rigidbody2D>().velocity = (Vector2)(_playerTransform.position - transform.position).normalized * fireBallProjectileSpeed;
        }
        private void Start()
        {
            _playerTransform = GameObject.FindWithTag("Player").transform;
            _collider2D = GetComponent<Collider2D>();
            _animator = GetComponentInChildren<Animator>();
            _navMeshAgent  = GetComponent<NavMeshAgent>();
            _behaviorTree = GetComponent<BehaviorTree>();
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.updateUpAxis = false;
            _fireSkullHealth = fireSkullInitialHealthReference.Value;
        }
        
        private void Death()
        {
            _collider2D.enabled = false;
            _behaviorTree.DisableBehavior();
            _animator.Play($"Death");
            Destroy(gameObject, 1f);
        }
    }
}