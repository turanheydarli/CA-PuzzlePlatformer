namespace Code.Scripts.StateMachines
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Trick(float deltaTime);
        public abstract void Exit();
    }
}