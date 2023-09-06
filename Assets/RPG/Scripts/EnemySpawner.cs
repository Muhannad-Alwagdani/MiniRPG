using System;
using System.Collections;
using UnityEngine;

namespace RPG.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float spawnRadius;

        private IEnumerator Start()
        {
            while (true)
            {
                //spawns enemy every 5 seconds
                yield return new WaitForSeconds(5f);
                var spawnPosition = transform.position + new Vector3(UnityEngine.Random.Range(-spawnRadius, spawnRadius), UnityEngine.Random.Range(-spawnRadius, spawnRadius), 0);
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}