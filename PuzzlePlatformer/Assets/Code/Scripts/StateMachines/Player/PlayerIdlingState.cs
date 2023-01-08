using Code.Scripts.Managers;

namespace Code.Scripts.StateMachines.Player
{
    public class PlayerIdlingState : PlayerBaseState
    {
        public PlayerIdlingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            CameraManager.Instance.OpenCamera("LevelStartCamera");
        }

        public override void Trick(float deltaTime)
        {
        }

        public override void Exit()
        {
            
        }
    }
}
