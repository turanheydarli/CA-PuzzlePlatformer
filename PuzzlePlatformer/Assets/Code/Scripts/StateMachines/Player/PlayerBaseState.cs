using Code.Scripts.Managers;
using UnityEngine;

namespace Code.Scripts.StateMachines.Player
{
    public abstract class PlayerBaseState : State
    {
        protected readonly PlayerStateMachine StateMachine;

        protected PlayerBaseState(PlayerStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }


        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            StateMachine.Controller.Move((motion + StateMachine.MovementHandler.Movement) * deltaTime);
        }

        protected void ReturnToLocomotion()
        {
            StateMachine.SwitchState(new PlayerMovingState(StateMachine));
        }

        protected Vector3 CalculateMovement()
        {
            Vector3 forward = StateMachine.MainCameraTransform.forward;
            Vector3 right = StateMachine.MainCameraTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            return forward * StateMachine.InputReader.MovementValue.y +
                   right * StateMachine.InputReader.MovementValue.x;
        }

        protected void SayTrick(string message)
        {
            DialogueManager.Instance.Say(message, 1, 1);
        }

        protected void SayMessage(string message)
        {
            DialogueManager.Instance.Say(message);
        }
    }
}