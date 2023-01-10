using Code.Scripts.Classes;
using Code.Scripts.Managers;
using DG.Tweening;

namespace Code.Scripts.StateMachines.Player
{
    public class PlayerSleepingState : PlayerBaseState
    {
        private CheckPoint _checkPoint;

        public PlayerSleepingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            _checkPoint = ESDataManager.Instance.GetCheckPoint();

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
            StateMachine.transform.DOJump(_checkPoint.jumpPoint, 0.5f, 1, 0.5f)
                .AppendCallback(ReturnToLocomotion)
                .OnComplete(() => { CameraManager.Instance.OpenCamera("FreeLookCamera"); });
        }
    }
}