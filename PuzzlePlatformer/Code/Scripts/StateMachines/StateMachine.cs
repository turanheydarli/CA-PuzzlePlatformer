using UnityEngine;

namespace Code.Scripts.StateMachines
{
    public class StateMachine : MonoBehaviour
    {
        private State _currentState;

        public void SwitchState(State newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }
        private void Update()
        {
            _currentState?.Trick(Time.deltaTime);
        }
    }
}
