using Code.Scripts.Classes;
using Code.Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.Level
{
    public class CaveExit: MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ESDataManager.Instance.SetCheckPoint(new CheckPoint
                {
                    jumpPoint = new Vector3(31.46f, -3.8f, 24.98f),
                    sleepPoint = new Vector3(31.46f, -3.8f, 24.98f),
                    isShip = false
                });
                
                CameraManager.Instance.OpenCamera("CaveExitCamera");

                DOTween.KillAll();
                UIManager.Instance.Load(5);
                SceneManager.LoadSceneAsync("Level1");
            }
        }
    }
}