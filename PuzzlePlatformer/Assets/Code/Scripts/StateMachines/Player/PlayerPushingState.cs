using UnityEngine;

namespace Code.Scripts.StateMachines.Player
{
    public class PlayerPushingState : PlayerBaseState
    {
        private readonly ControllerColliderHit _pullable;
        private Vector3 _direction;
        
        private Transform _canPull;

        public PlayerPushingState(PlayerStateMachine stateMachine, ControllerColliderHit pullable) : base(stateMachine)
        {
            _pullable = pullable;
        }

        
        public override void Enter()
        {
            StateMachine.InputReader.OnJump += Jump;
            
            StateMachine.PullableDetector.OnPullableDetect += HandlePullableDetect;
            StateMachine.PullableDetector.OnPullableLoose += HandlePullableLoose;
            StateMachine.InputReader.OnHold += Pull;
        }

        public override void Trick(float deltaTime)
        {
            _direction = CalculateMovement();


            if (_direction.magnitude > 0.01f)
            {
                Rigidbody pullableRigidbody = _pullable.rigidbody;

                float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(StateMachine.transform.eulerAngles.y, targetAngle,
                    ref StateMachine.turnSmoothVelocity,
                    StateMachine.smoothTurnTime);

                StateMachine.transform.rotation = Quaternion.Euler(0, angle, 0);

                if (pullableRigidbody != null && !pullableRigidbody.isKinematic)
                {
                    pullableRigidbody.velocity = _pullable.moveDirection * StateMachine.pushingForce;
                }

                Move(_direction * StateMachine.pushingMovementSpeed, deltaTime);
            }
            else
            {
                ReturnToLocomotion();
            }
        }

        public override void Exit()
        {
            StateMachine.InputReader.OnJump -= Jump;
            
            StateMachine.PullableDetector.OnPullableDetect -= HandlePullableDetect;
            StateMachine.PullableDetector.OnPullableLoose -= HandlePullableLoose;
            StateMachine.InputReader.OnHold -= Pull;
        }

        private void HandlePullableDetect(Transform pullable)
        {
            _canPull = pullable;
        }
        
        private void HandlePullableLoose(Transform pullable)
        {
            _canPull = null;
        }
        
        private void Jump()
        {
            StateMachine.SwitchState(new PlayerJumpingState(StateMachine));
        }

        private void Pull()
        {
            if (_canPull != null)
            {
                StateMachine.SwitchState(new PlayerPullingState(StateMachine, _canPull));
            }
        }
    }
}