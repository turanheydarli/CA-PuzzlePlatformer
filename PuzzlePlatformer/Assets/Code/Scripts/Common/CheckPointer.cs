using Code.Scripts.Classes;
using Code.Scripts.Managers;
using UnityEngine;

namespace Code.Scripts.Common
{
    public class CheckPointer : MonoBehaviour
    {
        [SerializeField] private Transform sleepPoint;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ESDataManager.Instance.SetCheckPoint(new CheckPoint
                {
                    jumpPoint = other.transform.position,
                    sleepPoint = sleepPoint.position
                });
            }
        }
    }
}