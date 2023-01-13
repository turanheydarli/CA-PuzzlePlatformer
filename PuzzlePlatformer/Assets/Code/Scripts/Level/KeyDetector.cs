using System;
using UnityEngine;

namespace Code.Scripts.Level
{
    public class KeyDetector : MonoBehaviour
    {
        public event Action<Transform> OnKeyDetect;
        public event Action<Transform> OnKeyLoose;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Key"))
            {
                OnKeyDetect?.Invoke(other.transform);
            }
        }

        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Key"))
            {
                OnKeyLoose?.Invoke(other.transform);
            }
        }
    }
}