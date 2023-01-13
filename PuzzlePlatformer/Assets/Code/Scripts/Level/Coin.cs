using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.Level
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private float moveDuration = 0.6f;
        [SerializeField] private float moveTo = 4;

        private Sequence _sequence;

        private void Start()
        {
            _sequence = DOTween.Sequence();
            _sequence.Join(transform.DOLocalMoveY(Random.Range(1, moveTo), moveDuration).SetLoops(int.MaxValue, LoopType.Yoyo));
        }

        public void Interact()
        {
            transform.DOJump(transform.position + (new Vector3(0, 2, 3) * 5f), 1, 1, 0.3f)
                .OnComplete(() =>
                {
                    _sequence.Kill();
                    Destroy(gameObject);
                });
        }
    }
}