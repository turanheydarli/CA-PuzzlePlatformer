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
                if (TryGetComponent(out PlayerStateMachine playerStateMachine))
                {
                    hit.transform.GetComponentInChildren<Chest>()?.Interact(playerStateMachine);
                }
            }
        }
    }
}