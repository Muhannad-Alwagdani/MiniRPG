using System;
using System.Collections;
using RPG.Scripts.Interfaces;
using RPG.Scripts.Projectiles;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Scripts.Player
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [SerializeField] private FloatReference playerMaxHealthReference;
        [SerializeField] private FloatReference playerHealthReference;
        [SerializeField] private FloatReference playerMaxManaReference;
        [SerializeField] private FloatReference playerManaReference;
        [SerializeField] private Vector2Reference playerMovementVector;
        [SerializeField] private float characterSpeed;
        [SerializeField] private float meleeAttackRange;
        [SerializeField] private float meleeAttackRadius;
        [SerializeField] private Animator animator;
                                                                               
        [Header("Hook"), Space(10f)] 
        //-- Hook
        [SerializeField] private GameObject hookGameObject;
        [SerializeField] private float hookSpeed;
        [SerializeField] private float hookMaxDistance;        
        [FormerlySerializedAs("hookPullSpeed")] [SerializeField] private float hookPullDuration;
        [SerializeField] private float hookManaCost;
        [SerializeField] private BoolReference hookDeployed;
        
        
        
        // events
        [SerializeField] private GameEvent onDeathEvent;
        [SerializeField] private GameEvent onWinEvent;
        [SerializeField] private FloatGameEvent cameraShakeEvent;
        [SerializeField] private GameEvent gotKeyEvent;

        private Rigidbody2D _rb2d;
        private Collider2D _collider2D;
        private UnityEngine.Camera _mainCamera;            
        private Hook _hook;

        private Vector2 _facingDirection;
        private bool _movementLocked;
        


        //Caching Animator params into variables to decrease string comparisons
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Velocity = Animator.StringToHash("Velocity");
        private static readonly int Attacking = Animator.StringToHash("Attacking");

        public void PerformMeleeAttackAnimation()
        {
            animator.ResetTrigger(Attacking);
            animator.SetTrigger(Attacking);
        }
        public void PerformMeleeAttackCollision()
        {
            //cast a circle collider in front of the player
            // to perform the applyDamage method for each individual game object that has the IDamageable interface withing the proximity of player attack range
            
            bool playerHitSomething = false; // this is to check if the player hit something to shake the camera
            foreach (Collider2D col2D in Physics2D.OverlapCircleAll(
                         transform.position + (Vector3)(_facingDirection * meleeAttackRange), meleeAttackRadius, ~LayerMask.GetMask("Player")))
            {
                IDamageable damageable = col2D.GetComponent<IDamageable>();
                if(damageable != null)
                {
                    damageable.ApplyDamage(hitDirection: col2D.transform.position - transform.position);
                    playerHitSomething = true;
                }
            }

            if (playerHitSomething)
            {
                cameraShakeEvent.Raise(0.1f);
            }
        }
        public void PerformSpecialAttack()
        {
            if(hookDeployed.Value)return; // Ignore if hook is already deployed
            if(playerManaReference.Value < hookManaCost)return; // Ignore if player doesn't have enough mana
            playerManaReference.Value -= hookManaCost;
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            Vector3 direction = (mousePosition - transform.position);
            
            _hook.HookMaxDistance = hookMaxDistance;
            _hook.PullSpeed = hookPullDuration;
            _hook.ShootHook(direction, hookSpeed);
        }
        public void ApplyDamage(float value = 1, Vector3 hitDirection = default)
        {
            _rb2d.velocity = hitDirection.normalized * 20f;
            StartCoroutine(KnockBack());
            playerHealthReference.Value -= value;
            if (playerHealthReference.Value <= 0)
            {
                Death();
            }
        }

        

        private void Start()
        {
            playerHealthReference.Value = playerMaxHealthReference.Value;
            playerManaReference.Value = 0f;
            
            _rb2d = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UnityEngine.Camera>();
            _facingDirection = Vector2.down;
            _movementLocked = false;
            
            //Hook
            _hook = hookGameObject.GetComponent<Hook>();
            _hook.Parent = transform;
            _hook.PullRange = (meleeAttackRange + meleeAttackRadius) * 1.5f;

        }

        private void Update()
        {
            RefillMana();
            if(_movementLocked)
                return;
            if (playerMovementVector.Value.magnitude > 0f)
            {
                _facingDirection = playerMovementVector.Value;
            }

            animator.SetFloat(Horizontal, playerMovementVector.Value.x);
            animator.SetFloat(Vertical, playerMovementVector.Value.y);
            animator.SetFloat(Velocity, playerMovementVector.Value.magnitude);
        }
        

        private void FixedUpdate()
        {
            if(_movementLocked)
                return;
            _rb2d.velocity = playerMovementVector.Value * (characterSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Health"))
            {
                Heal();
                Destroy(col.gameObject);
            }
            else if(col.CompareTag("Key"))
            {
                gotKeyEvent.Raise();
                Destroy(col.gameObject);
            }
            else if (col.CompareTag("Door"))
            {
                onWinEvent.Raise();
            }
        }

        private IEnumerator KnockBack()
        {
            
            _movementLocked = true;
            _collider2D.enabled = false; // this will cause a BUG if the player get knocked back toward wall direction
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
            _collider2D.enabled = true;
            _movementLocked = false;
        }

        private void Heal()
        {
            //Heal player and make sure it doesn't go over the max health
            playerHealthReference.Value += 10f;
            playerHealthReference.Value = Mathf.Min(playerHealthReference.Value, playerMaxHealthReference.Value);
        }
        private void RefillMana()
        {
            if (playerManaReference.Value < playerMaxManaReference.Value)
            {
                playerManaReference.Value += 1f * Time.deltaTime;
            }
        }
        private void Death()
        {
            onDeathEvent.Raise();
        }



        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position + (Vector3)(_facingDirection * meleeAttackRange), meleeAttackRadius);
        }
    }
}