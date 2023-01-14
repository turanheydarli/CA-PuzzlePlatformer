using Code.Scripts.Classes;
using Code.Scripts.Managers;
using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.StateMachines.Player
{
    public class PlayerSleepingState : PlayerBaseState
    {
        private CheckPoint _checkPoint;
        
        private static readonly int StartJumpHash = Animator.StringToHash("StartJumping");
        private static readonly int SittingHash = Animator.StringToHash("Sitting");
        private const float CrossFadeDuration = 0.1f;

        public PlayerSleepingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            _checkPoint = ESDataManager.Instance.GetCheckPoint();
            
            if (_checkPoint.isShip)
            {
                StateMachine.Animator.CrossFadeInFixedTime(SittingHash, CrossFadeDuration);
            }
            
            StateMachine.InputReader.OnJump += JumpToStart;
            StateMachine.transform.position = _checkPoint.sleepPoint;
            CameraManager.Instance.OpenCamera("StartLevelCamera");
        }

        public override void Trick(float deltaTime)
        {
        }

        public override void Exit()
        {
            StateMachine.InputReader.OnJump -= JumpToStart;
        }

        private void JumpToStart()
        {
            StateMachine.Animator.CrossFadeInFixedTime(StartJumpHash, CrossFadeDuration);
            StateMachine.transform.DOJump(_checkPoint.jumpPoint, 0.5f, 1, 1f)
                .AppendCallback(ReturnToLocomotion)
                .OnComplete(() => { CameraManager.Instance.OpenCamera("FreeLookCamera"); });
        }
    }
}