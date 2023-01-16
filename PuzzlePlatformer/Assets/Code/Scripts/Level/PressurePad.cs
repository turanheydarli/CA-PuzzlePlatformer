using System;
using Code.Scripts.Managers;
using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.Level
{
    public class PressurePad : MonoBehaviour
    {

        [SerializeField] private Transform button;

        public event Action OnButtonPress;
        public event Action OnButtonRelease;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Pushable"))
            {
                CameraManager.Instance.Discover("BridgePlatformCamera");
                button.DOLocalMoveY(-1, 0.2f);
                OnButtonPress?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Pushable"))
            {
                button.DOLocalMoveY(0, 0.2f);
                OnButtonRelease?.Invoke();
            }
        }
    }
}