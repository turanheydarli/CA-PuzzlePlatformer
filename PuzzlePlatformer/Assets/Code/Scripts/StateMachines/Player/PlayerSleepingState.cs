using Code.Scripts.Managers;
using DG.Tweening;

namespace Code.Scripts.StateMachines.Player
{
    public class PlayerSleepingState : PlayerBaseState
    {
        public PlayerSleepingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            StateMachine.InputReader.OnJump += JumpToStart;
            StateMachine.transform.position = StateMachine.CheckPoint.sleepPoint;
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
            StateMachine.transform.DOJump(StateMachine.CheckPoint.jumpPoint, 0.5f, 1, 0.5f)
                .AppendCallback(ReturnToLocomotion)
                .OnComplete(() => { CameraManager.Instance.OpenCamera("FreeLookCamera"); });
        }
    }
}