using System;
using UnityEngine;

namespace Code.Scripts
{
    public class ElevatorPad : MonoBehaviour
    {
        [SerializeField] private ElevatorHandler elevatorHandler;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                elevatorHandler.CallElevator();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                elevatorHandler.ReleaseElevator();
            }
        }
    }
}
