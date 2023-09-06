using System;
using UnityEngine;

namespace RPG.Scripts.Misc
{
    public class FireBallManager : MonoBehaviour
    {
        public static FireBallManager Instance;
        [SerializeField] private int numOfFireBalls;
        [SerializeField] private GameObject fireBallPrefab;
        [SerializeField] private Transform fireBallsContainer;

        private int _fireBallIndex;
        private GameObject[] _fireBalls;

        public GameObject GetFireBall() // to iterate over the pool of fireballs
        {
            GameObject fireball = _fireBalls[_fireBallIndex];
            _fireBallIndex = (_fireBallIndex + 1) % numOfFireBalls;
            return fireball;
        }
        private void Start()
        {
            Instance = this;
            _fireBallIndex = 0;
            _fireBalls = new GameObject[numOfFireBalls];
            for (int i = 0; i < numOfFireBalls; i++)
            {
                _fireBalls[i] = Instantiate(fireBallPrefab, transform);
                _fireBalls[i].transform.SetParent(fireBallsContainer);
                _fireBalls[i].SetActive(false);
            }
        }
    }
}