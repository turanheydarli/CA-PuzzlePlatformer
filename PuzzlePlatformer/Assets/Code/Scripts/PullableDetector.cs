using System;
using UnityEngine;

namespace Code.Scripts
{
    public class PullableDetector : MonoBehaviour
    {
        public event Action<Transform> OnPullableDetect;
        public event Action<Transform> OnPullableLoose;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Pullable"))
            {
                Debug.Log("Pullable Detected");
                OnPullableDetect?.Invoke(other.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Pullable"))
            {
                OnPullableLoose?.Invoke(other.transform);
            }
        }
    }
}
