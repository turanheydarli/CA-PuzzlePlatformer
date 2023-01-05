using UnityEngine;

namespace Code.Scripts.Movements
{
    public class ForceReceiver : MonoBehaviour, IMovementModifier
    {
        [SerializeField] private CharacterController controller;
        [SerializeField] private MovementHandler handler;

        [SerializeField] private float drag = 5f;
        [SerializeField] private float mass = 1f;

        public Vector3 Value { get; private set; }

        private bool _wasGroundedLastFrame;

        private void OnEnable() => handler.AddModifier(this);

        private void OnDisable() => handler.RemoveModifier(this);

        private void Update()
        {
            if (!_wasGroundedLastFrame && controller.isGrounded)
            {
                Value = new Vector3(Value.x, 0, Value.y);
            }

            _wasGroundedLastFrame = controller.isGrounded;

            if (Value.magnitude < 0.2f)
            {
                Value = Vector3.zero;
            }

            Value = Vector3.Lerp(Value, Vector3.zero, drag * Time.deltaTime);
        }

        public void AddForce(Vector3 force) => Value += force / mass;
    }
}