using Code.Scripts.Level;
using UnityEngine;

namespace Code.Scripts.StateMachines.Player
{
    public class PlayerFallingState : PlayerBaseState
    {
        private Vector3 _momentum;
        
        private readonly int FallHash = Animator.StringToHash("Falling");
        private const float CrossFadeDuration = 0.1f;


        public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            StateMachine.CollectableDetector.OnCollectableDetect += HandleCollectableDetect;
            
            StateMachine.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);

            _momentum = StateMachine.Controller.velocity;
            _momentum.y = 0f;
        }

        public override void Trick(float deltaTime)
        {
            Move(_momentum, deltaTime);

            if (StateMachine.Controller.isGrounded)
            {
                ReturnToLocomotion();
            }
        }

        public override void Exit()
        {
            StateMachine.CollectableDetector.OnCollectableDetect -= HandleCollectableDetect;
        }

        private void HandleCollectableDetect(Transform collectable)
        {
            collectable.GetComponent<Collectable>()?.Interact();
        }
    }
}