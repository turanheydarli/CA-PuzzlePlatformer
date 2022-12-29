using UnityEngine;

namespace Code.Scripts.StateMachines.Player
{
    public class PlayerPullingState : PlayerBaseState
    {
        private readonly Transform _pullable;
        private Vector3 _direction;

        public PlayerPullingState(PlayerStateMachine stateMachine, Transform pullable) : base(stateMachine)
        {
            _pullable = pullable;
        }

        public override void Enter()
        {
            StateMachine.InputReader.OnDrop += Drop;

            if (_pullable.parent.TryGetComponent(out Rigidbody connect))
                StateMachine.HolderJoint.connectedBody = connect;
        }

        public override void Trick(float deltaTime)
        {
            _direction = CalculateMovement();
            if (_direction.magnitude > 0.01f)
            {
                float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(StateMachine.transform.eulerAngles.y, targetAngle,
                    ref StateMachine.turnSmoothVelocity, StateMachine.pullingSmoothTurnTime);
                StateMachine.transform.rotation = Quaternion.Euler(0, angle, 0);

                Move(-(_direction * StateMachine.pullingMovementSpeed), deltaTime);
            }
        }

        public override void Exit()
        {
            StateMachine.HolderJoint.connectedBody = null;
            StateMachine.InputReader.OnDrop -= Drop;
        }

        private void Drop()
        {
            ReturnToLocomotion();
        }
    }
}