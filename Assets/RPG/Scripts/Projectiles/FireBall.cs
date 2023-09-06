using System.Collections;
using RPG.Scripts.Interfaces;
using UnityEngine;

namespace RPG.Scripts.Projectiles
{
    public class FireBall : MonoBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine(StartAging());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator StartAging() // this is a strange naming but what it does is just to make sure that the object gets disabled when it does not hit anything
        {
            yield return new WaitForSeconds(3f);
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if(col.gameObject.CompareTag("Player"))
                col.gameObject.GetComponent<IDamageable>().ApplyDamage();
            gameObject.SetActive(false);
        }
    }
}