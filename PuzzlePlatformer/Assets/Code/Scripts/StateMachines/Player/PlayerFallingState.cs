using UnityEngine;

namespace Code.Scripts.StateMachines.Player
{
    public class PlayerFallingState:PlayerBaseState
    {
        private Vector3 _momentum;

        public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            
        }

        public override void Enter()
        {
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
        }
    }
}