using UnityEngine;

namespace Code.Scripts.StateMachines.Player
{
    public class PlayerJumpingState : PlayerBaseState
    {
        private Vector3 _momentum;

        public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        
        public override void Enter()
        {
            StateMachine.ForceReceiver.Jump(StateMachine.jumpForce);
            _momentum = StateMachine.Controller.velocity;
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
        }
    }

}