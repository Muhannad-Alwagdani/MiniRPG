using System;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Scripts.UI
{
    public class PropertyContainer : MonoBehaviour
    {
        [SerializeField] private FloatReference propertyReferenceMaxValue;
        [SerializeField] private FloatReference propertyReferenceValue;
        
        
        [SerializeField] private Image propertyImage;

        private void Update()
        {
            propertyImage.fillAmount = propertyReferenceValue.Value / propertyReferenceMaxValue.Value;
        }
    }
}