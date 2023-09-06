using System;
using UnityEngine;

namespace RPG.Scripts
{
    public class Developer : MonoBehaviour
    {
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.R))
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}