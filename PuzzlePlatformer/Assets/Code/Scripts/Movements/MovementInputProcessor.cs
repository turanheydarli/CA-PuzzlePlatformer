using UnityEngine;

namespace Code.Scripts.Movements
{
    public class MovementInputProcessor : MonoBehaviour, IMovementModifier
    {
        [SerializeField] private CharacterController controller;
        [SerializeField] private MovementHandler handler;

        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float acceleration = 0.1f;

        private float _currentSpeed;
        private Vector3 _previousVelocity;
        private Vector3 _previousDirection;

        private Transform _mainCameraTransform;

        public Vector3 Value { get; private set; }

        private void Start() => _mainCameraTransform = Camera.main.transform;
        private void OnEnable() => handler.AddModifier(this);
        private void OnDisable() => handler.RemoveModifier(this);

        public void SetMovementInput(Vector2 direction)
        {
            _previousDirection = direction;
        }

        private void Update()
        {
            float targetSpeed = movementSpeed * _previousDirection.magnitude;

            _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, acceleration * Time.deltaTime);

            Vector3 forward = _mainCameraTransform.forward;
            Vector3 right = _mainCameraTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            Vector3 movementDirection;

            if (targetSpeed != 0)
            {
                movementDirection = forward * _previousDirection.y + right * _previousDirection.x;
            }
            else
            {
                movementDirection = _previousVelocity.normalized;
            }

            Value = movementDirection;

            var velocity = controller.velocity;

            _previousVelocity = new Vector3(velocity.x, 0, velocity.z);

            _currentSpeed = _previousVelocity.magnitude;
        }
    }
}