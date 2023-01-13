using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.DWAnimations
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Vector3 goTo;
        [SerializeField] private float duration;
        [SerializeField] private bool isAnimated;

        void Start()
        {
            Physics.autoSyncTransforms = true;
            
            if (isAnimated)
            {
                transform.DOLocalMove(goTo, duration).SetLoops(-1, LoopType.Yoyo);
            }
        }

        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.transform.parent = transform;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.transform.parent = null;
            }
        }
    }
}