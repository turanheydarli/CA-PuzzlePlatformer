using DG.Tweening;
using UnityEngine;

namespace Code.Scripts
{
    public class Lever : MonoBehaviour
    {
        [SerializeField] private LeverHandler leverHandler;
        [SerializeField] private Transform handle;

        public bool isOpened;

        public void Interact()
        {
            Debug.Log("Lever hitted");
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