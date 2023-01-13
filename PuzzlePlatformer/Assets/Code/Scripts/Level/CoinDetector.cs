using System;
using UnityEngine;

namespace Code.Scripts.Level
{
    public class CoinDetector : MonoBehaviour
    {
        public event Action<Transform> OnCoinDetect;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Coin"))
            {
                OnCoinDetect?.Invoke(other.transform);
            }
        }
    }
}
