using System;
using DG.Tweening;
using UnityEngine;

namespace Code.Scripts
{
    public class Hitable : MonoBehaviour
    {
        [SerializeField] private ElevatorHandler elevatorHandler;

        public bool isOpened;

        public void Interact()
        {
            isOpened = !isOpened;

            if (isOpened)
            {
                transform.DORotate(Vector3.forward * -30, 0.01f);
                elevatorHandler.CallElevator();
            }
            else
            {
                transform.DORotate(Vector3.forward * 30, 0.01f);
                elevatorHandler.ReleaseElevator();
            }
        }
    }
}