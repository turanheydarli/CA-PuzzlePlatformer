using Code.Scripts.Level;
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
            StateMachine.InputReader.OnJump += Jump;
            StateMachine.CoinDetector.OnCoinDetect += HandleCoinDetect;

            StateMachine.Animator.CrossFadeInFixedTime("Jumping", CrossFadeDuration);
            
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
            StateMachine.CoinDetector.OnCoinDetect -= HandleCoinDetect;
        }

        private void Jump()
        {
            StateMachine.SwitchState(new PlayerJumpingState(StateMachine));
        }

        private void HandleCoinDetect(Transform coin)
        {
            coin.GetComponent<Coin>()?.Interact();
        }
    }
}