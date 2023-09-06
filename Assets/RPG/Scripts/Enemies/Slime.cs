using System;
using System.Collections;
using BehaviorDesigner.Runtime;
using RPG.Scripts.Interfaces;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Scripts.Enemies
{
    public class Slime : MonoBehaviour,IDamageable, IHookable
    {
        public FloatReference slimeHealthReference;
        
        private Rigidbody2D _rb2d;
        private Collider2D _collider2D;
        private Animator _animator;
        private NavMeshAgent _navMeshAgent;
        private BehaviorTree _behaviorTree;
        private float _slimeHealth;

        public void ApplyDamage(float value = 1, Vector3 hitDirection = default)
        {
            _slimeHealth -= value;
            if (_slimeHealth <= 0)
            {
                Death();
            }
            else
            {
                _rb2d.velocity = hitDirection.normalized * 20f;
                StartCoroutine(KnockBack());
            }
        }
        private void Start()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            _animator = GetComponentInChildren<Animator>();
            _navMeshAgent  = GetComponent<NavMeshAgent>();
            _behaviorTree = GetComponent<BehaviorTree>();
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.updateUpAxis = false;
            _slimeHealth = slimeHealthReference.Value;
        }
        

        private void OnCollisionEnter2D(Collision2D col)
        {
            if(col.gameObject.CompareTag("Player"))
                col.gameObject.GetComponent<IDamageable>().ApplyDamage(hitDirection: col.transform.position - transform.position);
        }

        private void Death()
        {
            _collider2D.enabled = false;
            _behaviorTree.DisableBehavior();
            _navMeshAgent.enabled = false;
            _animator.Play($"Death");
            Destroy(gameObject, 1f);
        }

        private IEnumerator KnockBack()
        {
            
            float time = 0.5f;
            float appliedTimer = 0f;
            while (appliedTimer <= time)
            {
                //lerp velocity to zero
                _rb2d.velocity = Vector2.Lerp(_rb2d.velocity, Vector2.zero, appliedTimer / time);
                appliedTimer += Time.deltaTime;
                yield return null;
            }
            //set velocity to zero
            _rb2d.velocity = Vector2.zero;
        }


        public void Hook()
        {
            throw new NotImplementedException();
        }
    }
}