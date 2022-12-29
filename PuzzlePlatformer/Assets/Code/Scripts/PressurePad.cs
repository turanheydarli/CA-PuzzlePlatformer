using System;
using UnityEngine;

namespace Code.Scripts
{
    public class PressurePad : MonoBehaviour
    {
        public event Action OnButtonPress;
        public event Action OnButtonRelease;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Pushable"))
            {
                OnButtonPress?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Pushable"))
            {
                OnButtonRelease?.Invoke();
            }
        }
    }
}