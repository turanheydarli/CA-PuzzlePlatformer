using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.Level
{
    public class Lever : MonoBehaviour
    {
        [SerializeField] private LeverHandler leverHandler;
        [SerializeField] private Transform handle;

        public bool isOpened;

        public void Interact()
        {
            isOpened = !isOpened;

            if (isOpened)
            {
                handle.DORotate(Vector3.right * -30, 0.01f);
                leverHandler.HandleBridgeOpening();
            }
            else
            {
                handle.DORotate(Vector3.right * 30, 0.01f);
                leverHandler.HandleBridgeClosing();
            }
        }
    }
}