using System.Collections;
using Code.Scripts.Managers;
using UnityEngine;

namespace Code.Scripts.StateMachines.Player
{
    public class PlayerDiscoveringState : PlayerBaseState
    {
        private readonly string _cameraName;

        public PlayerDiscoveringState(PlayerStateMachine stateMachine, string cameraName) : base(stateMachine)
        {
            _cameraName = cameraName;
        }

        public override void Enter()
        {
            CameraManager.Instance.OpenCamera(_cameraName);
            StateMachine.StartCoroutine(ReturnToPlayer());
        }

        public override void Trick(float deltaTime)
        {
        }

        public override void Exit()
        {
            CameraManager.Instance.OpenCamera("FreeLookCamera");
        }

        IEnumerator ReturnToPlayer()
        {
            yield return new WaitForSeconds(3);
            ReturnToLocomotion();
        }
        
    }
}