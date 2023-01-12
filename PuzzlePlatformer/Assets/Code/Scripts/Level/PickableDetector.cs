using System;
using UnityEngine;

namespace Code.Scripts
{
    public class PickableDetector : MonoBehaviour
    {
        public event Action<Transform> OnPickableDetect;
        public event Action<Transform> OnPickableLoose;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Pickable"))
            {
                OnPickableDetect?.Invoke(other.transform);
            }
        }

        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Pickable"))
            {
                OnPickableLoose?.Invoke(other.transform);
            }
        }
    }
}