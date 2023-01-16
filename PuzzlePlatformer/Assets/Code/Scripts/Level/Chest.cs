using System.Collections;
using Code.Scripts.Managers;
using Code.Scripts.StateMachines.Player;
using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.Level
{
    public class Chest : MonoBehaviour
    {
        [SerializeField] private string cameraName;
        [SerializeField] Transform parchment;
        [SerializeField] Transform handles;
        [SerializeField] ParticleSystem openingParticle;

        private Animator _animator;
        private bool _isOpened;
        private Sequence _sequence;
        private static readonly int Opening = Animator.StringToHash("Opening");


        private void Start()
        {
            _sequence = DOTween.Sequence();
            _animator = GetComponentInChildren<Animator>();
        }

        public void Interact(PlayerStateMachine playerStateMachine)
        {
            if (_isOpened) return;

            if (playerStateMachine.HasKey)
            {
                playerStateMachine.HasKey = false;
                ESDataManager.Instance.gameData.hasKey = false;
                ESDataManager.Instance.gameData.chestCount++;
                playerStateMachine.ChestCount++;
                UIManager.Instance.SetChest(playerStateMachine.ChestCount);
                _isOpened = true;
                CameraManager.Instance.Discover(cameraName);
                StartCoroutine(StartOpeningAnimation());
            }
            else
            {
                DialogueManager.Instance.Say("Alas, we need a key to open the chest...");
            }
        }

        private IEnumerator StartOpeningAnimation()
        {
            yield return new WaitForSeconds(1f);
            _animator.SetTrigger(Opening);
            GetComponentInParent<BoxCollider>().center = new Vector3(-0.124f, 1.55f, 0.021f);
            handles.DOLocalMoveY(1.55f, 0);
        }

        public void Animate()
        {
            _sequence.Append(parchment.DOScale(1, 0.8f));
            _sequence.Join(parchment.DOLocalMoveY(1.36f, 0.8f));
            openingParticle.Play();
            SoundManager.Instance.Play("LeverSound");
            DOTween.Sequence(parchment.DORotate(Vector3.up * 180, 5f))
                .InsertCallback(1f, () => { UIManager.Instance.ShowParchment(); }).OnComplete(() =>
                {
                    _sequence.Kill();
                    parchment.DOScale(0, 0.3f).OnComplete(() => Destroy(parchment.gameObject));
                });
        }
    }
}