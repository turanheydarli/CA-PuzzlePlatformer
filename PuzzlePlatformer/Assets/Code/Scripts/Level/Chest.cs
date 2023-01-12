using System.Collections;
using Code.Scripts.Managers;
using UnityEngine;

namespace Code.Scripts.Level
{
    public class Chest : MonoBehaviour
    {
        [SerializeField] private string cameraName;

        private Animator _animator;
        private bool _isOpened;

        private static readonly int Opening = Animator.StringToHash("Opening");

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void Interact()
        {
            if (_isOpened) return;
            _isOpened = true;
            CameraManager.Instance.Discover(cameraName);
            StartCoroutine(StartOpeningAnimation());
        }

        private IEnumerator StartOpeningAnimation()
        {
            yield return new WaitForSeconds(1f);
            _animator.SetTrigger(Opening);
            _animator.transform.position -= new Vector3(0, -1, 0);
        }
    }
}