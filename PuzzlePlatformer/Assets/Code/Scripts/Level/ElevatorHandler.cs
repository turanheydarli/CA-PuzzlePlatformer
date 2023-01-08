using DG.Tweening;
using UnityEngine;

namespace Code.Scripts
{
    public class ElevatorHandler : MonoBehaviour
    {
        [SerializeField] private Vector3 goTo;
        [SerializeField] private float duration;
        
        private Vector3 _home;
        
        void Start()
        {
            _home = transform.position;
        }
        
        public void CallElevator()
        {
            transform.DOMove(goTo, duration);
        }
        
        public void ReleaseElevator()
        {
            transform.DOMove(_home, duration);
        }
        
        
    }
}
