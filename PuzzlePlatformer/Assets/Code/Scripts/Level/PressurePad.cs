using System;
using DG.Tweening;
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
                transform.DOScale(new Vector3(1, 0.1f, 1), 0.1f);
                OnButtonPress?.Invoke();
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Pushable"))
            {
                transform.DOScale(new Vector3(1, 1, 1), 0.1f);
                OnButtonRelease?.Invoke();
            }
        }
    }
}