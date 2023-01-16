using Code.Scripts.Managers;
using Code.Scripts.StateMachines.Player;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.Level
{
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            other.TryGetComponent(out PlayerStateMachine playerStateMachine);

            if (playerStateMachine.HealthCount <= 0)
            {
                UIManager.Instance.StartNewGame();
            }
            else
            {
                playerStateMachine.HealthCount--;
                UIManager.Instance.SetHealth(playerStateMachine.HealthCount);
            }

            DOTween.KillAll();

            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}