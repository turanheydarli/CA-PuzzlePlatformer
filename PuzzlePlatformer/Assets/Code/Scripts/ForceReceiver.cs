using UnityEngine;

namespace Code.Scripts
{
    public class ForceReceiver : MonoBehaviour
    {
        [SerializeField] private CharacterController controller;
        [SerializeField] private float drag = 0.3f;

        private Vector3 _dampingVelocity;
        private Vector3 _impact;
        private float _verticalVelocity;

        public Vector3 Movement => _impact + Vector3.up * _verticalVelocity;

        private void Update()
        {
            if (_verticalVelocity < 0f && controller.isGrounded)
            {
                _verticalVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                _verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }

            _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, drag);
            
        }
        

        public void Reset()
        {
            _impact = Vector3.zero;
            _verticalVelocity = 0f;
        }

        public void AddForce(Vector3 force)
        {
            _impact += force;
        }

        public void Jump(float jumpForce)
        {
            _verticalVelocity += jumpForce;
        }
    }
}