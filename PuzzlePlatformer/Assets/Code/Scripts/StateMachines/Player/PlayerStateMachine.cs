using Code.Scripts.Common;
using Code.Scripts.Level;
using Code.Scripts.Managers;
using Code.Scripts.Movements;
using UnityEngine;

namespace Code.Scripts.StateMachines.Player
{
    public class PlayerStateMachine : StateMachine
    {
        [SerializeField] public float jumpForce = 3f;
        [SerializeField] public float smoothTurnTime = 0.05f;
        [SerializeField] public float movementSpeed = 10;
        [SerializeField] public float acceleration = 1;
        [SerializeField] public float currentSpeed;

        [SerializeField] public float turnSmoothVelocity = 0.3f;
        [SerializeField] public float pushingForce = 3f;
        [SerializeField] public float pushingMovementSpeed = 5f;
        [SerializeField] public float pullingMovementSpeed = 3f;
        [SerializeField] public float pullingSmoothTurnTime = 0.07f;

        public Transform PickedItem { get; set; }
        public int StrawberryCount { get; set; }
        public int HealthCount { get; set; }
        public int ChestCount { get; set; } 
        public bool HasKey { get; set; }
        public bool KnowsPush { get; set; }
        [field: SerializeField] public HingeJoint HolderJoint { get; private set; }
        public InputReader InputReader { get; private set; }
        public CharacterController Controller { get; private set; }
        public Transform MainCameraTransform { get; private set; }
        public MovementHandler MovementHandler { get; private set; }
        public ForceReceiver ForceReceiver { get; private set; }
        public PushableDetector PushableDetector { get; private set; }
        public PullableDetector PullableDetector { get; private set; }
        public HitableDetector HitableDetector { get; private set; }
        public PickableDetector PickableDetector { get; private set; }
        public CollectableDetector CollectableDetector { get; private set; }
        public KeyDetector KeyDetector { get; private set; }
        public Animator Animator { get; private set; }

        private void Awake()
        {
            InputReader = GetComponent<InputReader>();
            Controller = GetComponent<CharacterController>();
            ForceReceiver = GetComponent<ForceReceiver>();
            MovementHandler = GetComponent<MovementHandler>();
            Animator = GetComponent<Animator>();

            HitableDetector = FindObjectOfType<HitableDetector>();
            PushableDetector = FindObjectOfType<PushableDetector>();
            PullableDetector = FindObjectOfType<PullableDetector>();
            PickableDetector = FindObjectOfType<PickableDetector>();
            CollectableDetector = FindObjectOfType<CollectableDetector>();
            
            KeyDetector = FindObjectOfType<KeyDetector>();
            StrawberryCount = ESDataManager.Instance.gameData.strawberryCount;
            HealthCount = ESDataManager.Instance.gameData.healthCount;
            ChestCount = ESDataManager.Instance.gameData.chestCount;
            HasKey = ESDataManager.Instance.gameData.hasKey;
            
            MainCameraTransform = Camera.main.transform;
        }

        private void Start()
        {
            SwitchState(new PlayerSleepingState(this));
        }
    }
}