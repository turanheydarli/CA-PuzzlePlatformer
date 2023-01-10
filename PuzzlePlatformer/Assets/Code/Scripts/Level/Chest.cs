using System.Collections;
using Code.Scripts.Managers;
using UnityEngine;

namespace Code.Scripts.Level
{
    public class Chest : MonoBehaviour
    {
        [SerializeField] private string cameraName;

        private Animator _animator;

        private static readonly int Opening = Animator.StringToHash("Opening");

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Key"))
            {
                collision.transform.tag = "Untagged";
                
                CameraManager.Instance.Discover(cameraName);
                
                StartCoroutine(StartOpeningAnimation());
            }
        }

        private IEnumerator StartOpeningAnimation()
        {
            yield return new WaitForSeconds(1f);
            _animator.SetTrigger(Opening);
        }
    }
}