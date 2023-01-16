using Code.Scripts.Level;
using Code.Scripts.Managers;
using UnityEngine;

namespace Code.Scripts.StateMachines.Player
{
    public class PlayerJumpingState : PlayerBaseState
    {
        private Vector3 _momentum;

        private const float CrossFadeDuration = 0.1f;

        private static readonly int JumpHash = Animator.StringToHash("Jumping");

        public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }


        public override void Enter()
        {
            StateMachine.KeyDetector.OnKeyDetect += HandleKeyDetect;

            StateMachine.InputReader.OnJump += Jump;
            StateMachine.CollectableDetector.OnCollectableDetect += HandleCollectableDetect;

            StateMachine.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);

            StateMachine.ForceReceiver.AddForce(Vector3.up * StateMachine.jumpForce);
            _momentum = StateMachine.Controller.velocity;
            _momentum += CalculateMovement();
            _momentum.y = 0f;
        }


        public override void Trick(float deltaTime)
        {
            Move(_momentum, deltaTime);

            if (StateMachine.Controller.velocity.y <= 0)
            {
                StateMachine.SwitchState(new PlayerFallingState(StateMachine));
            }
        }

        public override void Exit()
        {
            StateMachine.InputReader.OnJump -= Jump;
            StateMachine.CollectableDetector.OnCollectableDetect -= HandleCollectableDetect;
            StateMachine.KeyDetector.OnKeyDetect -= HandleKeyDetect;
        }

        private void Jump()
        {
            StateMachine.SwitchState(new PlayerJumpingState(StateMachine));
        }

        private void HandleCollectableDetect(Transform collectable)
        {
            StateMachine.StrawberryCount++;
            UIManager.Instance.SetCollectable(StateMachine.StrawberryCount);
            ESDataManager.Instance.gameData.strawberryCount = StateMachine.StrawberryCount;
            SoundManager.Instance.Play("EatingSound");
            collectable.GetComponent<Collectable>()?.Interact();
        }

        private void HandleKeyDetect(Transform key)
        {
            if (StateMachine.HasKey) return;
            StateMachine.HasKey = true;
            ESDataManager.Instance.gameData.hasKey = true;
            key.GetComponent<Key>()?.Interact(StateMachine.HolderJoint.transform);
        }
    }
}