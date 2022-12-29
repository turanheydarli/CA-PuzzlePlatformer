using UnityEngine;

namespace Code.Scripts.StateMachines.Player
{
    public class PlayerStateMachine : StateMachine
    {
        [SerializeField] public float jumpForce = 3f;
        [SerializeField] public float smoothTurnTime = 0.05f;
        [SerializeField] public float movementSpeed = 10;
        [SerializeField] public float turnSmoothVelocity = 0.3f;
        [SerializeField] public float pushingForce = 3f;
        [SerializeField] public float pushingMovementSpeed = 5f;
        [SerializeField] public float pullingMovementSpeed = 3f;
        [SerializeField] public float pullingSmoothTurnTime =  0.07f;

        [field: SerializeField] public HingeJoint HolderJoint { get; private set; }

        public Transform PickedItem { get; set; }
        
        public InputReader InputReader { get; private set; }
        public CharacterController Controller { get; private set; }
        public Transform MainCameraTransform { get; private set; }
        public ForceReceiver ForceReceiver { get; private set; }
        public PushableDetector PushableDetector { get; private set; }
        public PullableDetector PullableDetector { get; private set; }
        public HitableDetector HitableDetector { get; private set; }
        public PickableDetector PickableDetector { get; private set; }

        private void Awake()
        {
            InputReader = GetComponent<InputReader>();
            Controller = GetComponent<CharacterController>();
            ForceReceiver = GetComponent<ForceReceiver>();
            HitableDetector = FindObjectOfType<HitableDetector>();
            PushableDetector = FindObjectOfType<PushableDetector>();
            PullableDetector = FindObjectOfType<PullableDetector>();
            PickableDetector = FindObjectOfType<PickableDetector>();
                
            MainCameraTransform = Camera.main.transform;
        }

        private void Start()
        {
            SwitchState(new PlayerMovingState(this));
        }
    }
}