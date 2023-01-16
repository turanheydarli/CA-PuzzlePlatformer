using Code.Scripts.Managers;
using Code.Scripts.StateMachines.Player;
using UnityEngine;

namespace Level
{
    public class End : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent(out PlayerStateMachine playerStateMachine) &&
                    playerStateMachine.ChestCount == 3)
                {
                    UIManager.Instance.ShowEnd();
                }
                else if (playerStateMachine.ChestCount == 1)
                {
                    DialogueManager.Instance.Say("Our adventure is still unfinished.");
                }
            }
        }
    }
}