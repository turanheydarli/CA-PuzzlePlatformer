using System;
using UnityEngine;

namespace Code.Scripts.Level
{
    public class CollectableDetector : MonoBehaviour
    {
        public event Action<Transform> OnCollectableDetect;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Collectable"))
            {
                OnCollectableDetect?.Invoke(other.transform);
            }
        }
    }
}
