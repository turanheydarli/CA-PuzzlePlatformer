﻿using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.StateMachines.Player
{
    public class PlayerMovingState : PlayerBaseState
    {
        private Vector3 _direction;

        private Transform _canPick;
        private Transform _canHit;
        private Transform _canPull;
        

        public PlayerMovingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            StateMachine.InputReader.OnJump += Jump;
            StateMachine.InputReader.OnHold += Pick;
            StateMachine.InputReader.OnDrop += Drop;

            StateMachine.PushableDetector.OnPushableDetect += HandlePushableDetect;
            
            StateMachine.PickableDetector.OnPickableDetect += HandlePickableDetect;
            StateMachine.PickableDetector.OnPickableLoose += HandlePickableLoose;
            
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

            if (_direction.magnitude > 0.01f)
            {
                float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(StateMachine.transform.eulerAngles.y, targetAngle,
                    ref StateMachine.turnSmoothVelocity, StateMachine.smoothTurnTime);

                StateMachine.transform.rotation = Quaternion.Euler(0, angle, 0);

                Move(_direction * StateMachine.movementSpeed, deltaTime);
            }
        }

        public override void Exit()
        {
            StateMachine.InputReader.OnJump -= Jump;
            StateMachine.InputReader.OnHold -= Pick;
            StateMachine.InputReader.OnDrop -= Drop;

            StateMachine.PushableDetector.OnPushableDetect -= HandlePushableDetect;
            StateMachine.PickableDetector.OnPickableDetect -= HandlePickableDetect;
            StateMachine.PickableDetector.OnPickableLoose -= HandlePickableLoose;
            
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

        private void HandlePushableDetect(ControllerColliderHit pushable)
        {
            StateMachine.SwitchState(new PlayerPushingState(StateMachine, pushable));
        }

        private void HandlePickableDetect(Transform pickable)
        {
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
            _canPull = pullable;
        }
        
        private void HandlePullableLoose(Transform pullable)
        {
            _canPull = null;
        }
        
        private void Drop()
        {
            if (StateMachine.PickedItem != null)
            {
                StateMachine.PickedItem.GetComponent<Rigidbody>().isKinematic = false;
                StateMachine.PickedItem.parent = null;
            }
        }

        private void Pick()
        {
            if (_canPick != null)
            {
                StateMachine.PickedItem = _canPick;

                StateMachine.PickedItem.parent = StateMachine.HolderJoint.transform;

                StateMachine.PickedItem.transform.DOLocalJump(Vector3.zero, 0.5f, 1, 0.3f);

                StateMachine.PickedItem.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        
        private void Hit()
        {
            if (_canHit != null)
            {
                _canHit.GetComponent<Hitable>()?.Interact();
            }
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