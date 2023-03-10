using Code.Scripts.Level;
using Code.Scripts.Managers;
using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.StateMachines.Player
{
    public class PlayerMovingState : PlayerBaseState
    {
        private Vector3 _direction;
        private Vector3 _previousVelocity;

        private Transform _canPick;
        private Transform _canHit;
        private Transform _canPull;

        private static readonly int RunningSpeed = Animator.StringToHash("RunningSpeed");
        private static readonly int MovingBlendTreeHash = Animator.StringToHash("MovingBlendTree");
        private const float CrossFadeDuration = 0.1f;


        public PlayerMovingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            StateMachine.Animator.CrossFadeInFixedTime(MovingBlendTreeHash, CrossFadeDuration);
            CameraManager.Instance.OpenCamera("FreeLookCamera");

            StateMachine.InputReader.OnJump += Jump;
            StateMachine.InputReader.OnHold += Pick;
            StateMachine.InputReader.OnDrop += Drop;

            StateMachine.PushableDetector.OnPushableDetect += HandlePushableDetect;
            StateMachine.CollectableDetector.OnCollectableDetect += HandleCollectableDetect;


            StateMachine.PickableDetector.OnPickableDetect += HandlePickableDetect;
            StateMachine.PickableDetector.OnPickableLoose += HandlePickableLoose;

            StateMachine.KeyDetector.OnKeyDetect += HandleKeyDetect;

            StateMachine.HitableDetector.OnHitableDetect += HandleHitableDetect;
            StateMachine.HitableDetector.OnHitableLoose += HandleHitableLoose;
            StateMachine.InputReader.OnHit += Hit;

            StateMachine.PullableDetector.OnPullableDetect += HandlePullableDetect;
            StateMachine.PullableDetector.OnPullableLoose += HandlePullableLoose;
            StateMachine.InputReader.OnHold += Pull;
        }

        public override void Trick(float deltaTime)
        {
            _direction = CalculateMovement();

            float targetSpeed = StateMachine.movementSpeed * _direction.magnitude;

            StateMachine.currentSpeed = Mathf.MoveTowards(StateMachine.currentSpeed, targetSpeed,
                StateMachine.acceleration * deltaTime);

            if (_direction.magnitude > 0.01f)
            {
                float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(StateMachine.transform.eulerAngles.y, targetAngle,
                    ref StateMachine.turnSmoothVelocity, StateMachine.smoothTurnTime);

                StateMachine.transform.rotation = Quaternion.Euler(0, angle, 0);
            }

            Move(_direction * StateMachine.currentSpeed, deltaTime);

            StateMachine.Animator.SetFloat(RunningSpeed, StateMachine.currentSpeed);

            Vector3 velocity = StateMachine.Controller.velocity;

            if (velocity.y < -5f)
            {
                StateMachine.SwitchState(new PlayerFallingState(StateMachine));
            }

            _previousVelocity = new Vector3(velocity.x, 0, velocity.z);

            StateMachine.currentSpeed = _previousVelocity.magnitude;
        }

        public override void Exit()
        {
            StateMachine.InputReader.OnJump -= Jump;
            StateMachine.InputReader.OnHold -= Pick;
            StateMachine.InputReader.OnDrop -= Drop;

            StateMachine.CollectableDetector.OnCollectableDetect -= HandleCollectableDetect;
            StateMachine.PushableDetector.OnPushableDetect -= HandlePushableDetect;

            StateMachine.PickableDetector.OnPickableDetect -= HandlePickableDetect;
            StateMachine.PickableDetector.OnPickableLoose -= HandlePickableLoose;

            StateMachine.KeyDetector.OnKeyDetect -= HandleKeyDetect;

            StateMachine.HitableDetector.OnHitableDetect -= HandleHitableDetect;
            StateMachine.HitableDetector.OnHitableLoose -= HandleHitableLoose;
            StateMachine.InputReader.OnHit -= Hit;

            StateMachine.PullableDetector.OnPullableDetect -= HandlePullableDetect;
            StateMachine.PullableDetector.OnPullableLoose -= HandlePullableLoose;
            StateMachine.InputReader.OnHold -= Pull;
        }

        private void Jump()
        {
            StateMachine.SwitchState(new PlayerJumpingState(StateMachine));
        }

        private void HandleCollectableDetect(Transform collectable)
        {
            StateMachine.StrawberryCount++;
            UIManager.Instance.SetCollectable(StateMachine.StrawberryCount);
            SoundManager.Instance.Play("EatingSound");
            ESDataManager.Instance.gameData.strawberryCount = StateMachine.StrawberryCount;
            collectable.GetComponent<Collectable>()?.Interact();
        }

        private void HandlePushableDetect(ControllerColliderHit pushable)
        {
            if (!StateMachine.KnowsPush)
            {
                SayMessage("You are strong enough, you can push some of these objects.");
                StateMachine.KnowsPush = true;
            }

            StateMachine.SwitchState(new PlayerPushingState(StateMachine, pushable));
        }

        private void HandlePickableDetect(Transform pickable)
        {
            SayTrick("Tap x to pick.");

            _canPick = pickable;
        }

        private void HandlePickableLoose(Transform pickable)
        {
            _canPick = null;
        }

        private void HandleHitableDetect(Transform hitable)
        {
            _canHit = hitable;
        }

        private void HandleHitableLoose(Transform hitable)
        {
            _canHit = null;
        }

        private void HandlePullableDetect(Transform pullable)
        {
            SayMessage("Press X and let's see what happens?");
            _canPull = pullable;
        }

        private void HandlePullableLoose(Transform pullable)
        {
            _canPull = null;
        }

        private void HandleKeyDetect(Transform key)
        {
            if (StateMachine.HasKey) return;
            StateMachine.HasKey = true;
            ESDataManager.Instance.gameData.hasKey = true;
            key.GetComponent<Key>()?.Interact(StateMachine.HolderJoint.transform);
        }

        private void Drop()
        {
            if (StateMachine.PickedItem == null) return;
            StateMachine.PickedItem.GetComponent<Rigidbody>().isKinematic = false;
            StateMachine.PickedItem.parent = null;
        }

        private void Pick()
        {
            if (_canPick == null) return;
            StateMachine.PickedItem = _canPick;
            StateMachine.PickedItem.parent = StateMachine.HolderJoint.transform;
            StateMachine.PickedItem.transform.DOLocalJump(Vector3.zero, 0.5f, 1, 0.3f);
            StateMachine.PickedItem.GetComponent<Rigidbody>().isKinematic = true;
        }

        private void Hit()
        {
            if (_canHit == null) return;
            SoundManager.Instance.Play("LeverSound");
            _canHit.GetComponent<Hitable>()?.Interact();
            _canHit.GetComponent<Lever>()?.Interact();
        }

        private void Pull()
        {
            if (_canPull == null) return;
            StateMachine.SwitchState(new PlayerPullingState(StateMachine, _canPull));
        }
    }
}