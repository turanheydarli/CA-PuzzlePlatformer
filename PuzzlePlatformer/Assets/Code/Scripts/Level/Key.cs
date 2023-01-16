using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.Level
{
    public class Key : MonoBehaviour
    {
        private Transform _transform;

        private void Start()
        {
            _transform = transform;
        }

        public void Interact(Transform holdTransform)
        {
            _transform.tag = "Untagged";
            _transform.parent = holdTransform;
            _transform.DOLocalJump(Vector3.zero, 0.5f, 1, 0.3f)
                .Join(_transform.DOScale(Vector3.zero, 0.3f))
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                });
        }
    }
}