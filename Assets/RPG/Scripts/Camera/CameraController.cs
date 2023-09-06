using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace RPG.Scripts.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float amplitude;
        [SerializeField] private float frequency;
        private CinemachineVirtualCamera _vcam;
        private CinemachineBasicMultiChannelPerlin _noise;

        public void ShakeCamera(float duration)
        {
            StartCoroutine(ShakeCamera_Co(duration));
        }
        
        private void Start()
        {
            _vcam = GetComponent<CinemachineVirtualCamera>();
            _noise = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private IEnumerator ShakeCamera_Co(float duration)
        {
            _noise.m_AmplitudeGain = amplitude;
            _noise.m_FrequencyGain = frequency;
            yield return new WaitForSeconds(duration);
            _noise.m_AmplitudeGain = 0;
            _noise.m_FrequencyGain = 0;
            
        }
    }
}