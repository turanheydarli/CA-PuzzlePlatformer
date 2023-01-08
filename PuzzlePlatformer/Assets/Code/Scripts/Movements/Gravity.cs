using UnityEngine;

namespace Code.Scripts.Movements
{
    public class Gravity : MonoBehaviour, IMovementModifier
    {
        [SerializeField] private MovementHandler handler;
        [SerializeField] private CharacterController controller;

        public Vector3 Value { get; private set; }

        [SerializeField] private float groundedPullMagnitude = 50f;

        private readonly float _gravityMagnitude = Physics.gravity.y;

        private bool _wasGroundedLastFrame;

        private void OnEnable() => handler.AddModifier(this);

        private void OnDisable() => handler.RemoveModifier(this);

        private void Update() => ProcessGravity();

        private void ProcessGravity()
        {
            if (controller.isGrounded)
            {
                Value = new Vector3(Value.x, -groundedPullMagnitude, Value.z);
            }
            else if (_wasGroundedLastFrame)
            {
                Value = Vector3.zero;
            }
            else
            {
                Value = new Vector3(Value.x, Value.y + _gravityMagnitude * Time.deltaTime, Value.z);
            }

            _wasGroundedLastFrame = controller.isGrounded;
        }
    }
}