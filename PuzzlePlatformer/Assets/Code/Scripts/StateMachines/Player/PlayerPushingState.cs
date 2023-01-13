using UnityEngine;

namespace Code.Scripts.StateMachines.Player
{
    public class PlayerPushingState : PlayerBaseState
    {
        private Vector3 _direction;
        private Transform _canPull;
        private readonly ControllerColliderHit _pushable;

        private readonly int PushHash = Animator.StringToHash("Pushing");

        public PlayerPushingState(PlayerStateMachine stateMachine, ControllerColliderHit pushable) : base(stateMachine)
        {
            _pushable = pushable;
        }


        public override void Enter()
        {
            StateMachine.InputReader.OnJump += Jump;

            StateMachine.Animator.SetBool(PushHash, true);

            StateMachine.PushableDetector.OnPushableDetect += Push;

            StateMachine.PullableDetector.OnPullableDetect += HandlePullableDetect;
            StateMachine.PullableDetector.OnPullableLoose += HandlePullableLoose;
            StateMachine.InputReader.OnHold += Pull;
        }

        public override void Trick(float deltaTime)
        {
            _direction = CalculateMovement();

            if (_pushable.rigidbody.velocity.magnitude <= 0.01f)
            {
                ReturnToLocomotion();
            }

            if (_direction.magnitude > 0.01f)
            {
                float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(StateMachine.transform.eulerAngles.y, targetAngle,
                    ref StateMachine.turnSmoothVelocity,
                    StateMachine.smoothTurnTime);

                StateMachine.transform.rotation = Quaternion.Euler(0, angle, 0);

                Move(_direction * StateMachine.pushingMovementSpeed, deltaTime);
            }
        }

        public override void Exit()
        {
            StateMachine.InputReader.OnJump -= Jump;
            
            StateMachine.Animator.SetBool(PushHash, false);

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

        private void Push(ControllerColliderHit hit)
        {
            Rigidbody pushableRigidbody = _pushable.rigidbody;

            if (pushableRigidbody != null && !pushableRigidbody.isKinematic)
            {
                pushableRigidbody.velocity = _pushable.moveDirection * StateMachine.pushingForce;
            }
        }
    }
}