using System.Collections;
using RPG.Scripts.Interfaces;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace RPG.Scripts.Projectiles
{
    public class Hook : MonoBehaviour
    {
        public Transform Parent { get; set; }
        public float HookMaxDistance { private get; set; }
        public float PullRange { private get; set; }
        public float PullSpeed { private get; set; }
        [SerializeField] private BoolReference hookDeployed;
        [SerializeField]private  Rigidbody2D rb2d;
        [SerializeField]private LineRenderer lineRenderer;
        
        [SerializeField]private Collider2D collider2D;
        private Vector3 _initialPosition;
        private bool _isHookReturning;

        public void ShootHook(Vector3 direction, float force)
        {
            gameObject.SetActive(true);
            rb2d.velocity = direction.normalized * force;
            transform.up = rb2d.velocity.normalized;
        }

        private void OnEnable()
        {
            hookDeployed.Value = true;
            collider2D.enabled = true;
            _initialPosition = Parent.position;
            transform.position = _initialPosition;
            lineRenderer.SetPosition(0, Parent.transform.position);
            lineRenderer.SetPosition(1, transform.position);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            hookDeployed.Value = false;
            _isHookReturning = false;
        }
        private void Update()
        {
            
            if(_isHookReturning)
                return;
            lineRenderer.SetPosition(0, Parent.transform.position);
            lineRenderer.SetPosition(1, transform.position);
            if (Vector2.SqrMagnitude(transform.position - _initialPosition) > HookMaxDistance * HookMaxDistance)
            {
                StartCoroutine(Return());
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            IHookable hookedObject = col.gameObject.GetComponent<IHookable>();
            if (hookedObject != null)
            {
                StartCoroutine(Return(col.rigidbody));
            }
            else
            {
                StartCoroutine(Return());

            }
        }
        
        private IEnumerator Return(Rigidbody2D hookedObject = null)
        {
            collider2D.enabled = false;
            rb2d.velocity = Vector2.zero;
            _isHookReturning = true;
            if (hookedObject)//if hook hit something
            {
                hookedObject.transform.parent = transform;
            }
            float timeToReturn = PullSpeed;
            float appliedTimer = 0f;
            Vector3 originalPosition = transform.position;
            while (appliedTimer <= timeToReturn)
            {
                lineRenderer.SetPosition(0, Parent.transform.position);
                lineRenderer.SetPosition(1, transform.position);
                transform.position = Vector3.Lerp(originalPosition, Parent.position + (transform.position - Parent.position).normalized * PullRange, appliedTimer / timeToReturn);
                appliedTimer += Time.deltaTime;
                yield return null;
            }

            if (hookedObject)//if hook hit something
            {
                hookedObject.transform.parent = null;
            }
            gameObject.SetActive(false);
        }
    }
}