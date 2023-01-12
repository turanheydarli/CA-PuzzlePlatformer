using Code.Scripts.StateMachines.Player;
using UnityEngine;

namespace Code.Scripts.Level
{
    public class Detector : MonoBehaviour
    {
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.transform.CompareTag("Pushable"))
            {
                if (TryGetComponent(out PlayerStateMachine playerStateMachine) && playerStateMachine.HasKey)
                {
                    hit.transform.GetComponent<Chest>()?.Interact();
                    playerStateMachine.HasKey = false;
                }
            }
        }
    }
}