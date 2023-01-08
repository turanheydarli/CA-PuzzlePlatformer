using System;
using UnityEngine;

namespace Code.Scripts
{
    public class HitableDetector : MonoBehaviour
    {
        public event Action<Transform> OnHitableDetect;
        public event Action<Transform> OnHitableLoose;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hitable"))
            {
                OnHitableDetect?.Invoke(other.transform);
            }
        }
        

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Hitable"))
            {
                OnHitableLoose?.Invoke(other.transform);
            }
        }
    }
}
