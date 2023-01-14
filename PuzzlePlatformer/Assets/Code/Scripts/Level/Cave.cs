using System;
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
                    jumpPoint = new Vector3(0, 3, 3),
                    sleepPoint = new Vector3(0, 3, 3),
                    isShip = false
                });
                
                DOTween.KillAll();
                
                SceneManager.LoadSceneAsync("Level1_Cave");
            }
        }
    }
}