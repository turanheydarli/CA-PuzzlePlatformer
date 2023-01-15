using Code.Scripts.Classes;
using Code.Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.Level
{
    public class Cave : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ESDataManager.Instance.SetCheckPoint(new CheckPoint
                {
                    jumpPoint = new Vector3(0.05f, 0.3f, 2.03f),
                    sleepPoint = new Vector3(0.05f, 0.3f, 2.03f),
                    isShip = false
                });

                DOTween.KillAll();
                UIManager.Instance.Load(5);
                SceneManager.LoadSceneAsync("Level1_Cave");
            }
        }
    }
}