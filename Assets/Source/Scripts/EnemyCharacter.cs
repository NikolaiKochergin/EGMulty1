using UnityEngine;

namespace Source.Scripts
{
    public class EnemyCharacter : Character
    {
        [SerializeField] private Transform _head;
        [SerializeField] private CapsuleCollider _collider;
        
        private float _velocityMagnitude = 0f;
        
        public Vector3 TargetPosition { get; private set; } = Vector3.zero;
        public Vector2 TargetRotation { get; private set; } = Vector2.zero;

        private void Start()
        {
            TargetPosition = transform.position;
            TargetRotation = new Vector2(_head.localEulerAngles.x, transform.localEulerAngles.y);
        }

        private void Update()
        {
            if (_velocityMagnitude > 0.01f)
            {
                float maxDistance = _velocityMagnitude * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, TargetPosition, maxDistance);
            }
            else
            {
                transform.position = TargetPosition;
            }
        }

        public void SetSpeed(float value) => Speed = value;

        public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval)
        {
            TargetPosition = position + velocity * averageInterval;
            _velocityMagnitude = velocity.magnitude;
            
            Velocity = velocity;
        }

        public void SetRotation(in Vector2 value)
        {
            _head.localEulerAngles = new Vector3(value.x, 0, 0);
            transform.localEulerAngles = new Vector3(0, value.y, 0);
        }

        public void SetCrouch(bool crouchInfoIsCrch)
        {
            IsCrouch = crouchInfoIsCrch;
            if (crouchInfoIsCrch)
            {
                _collider.center = new Vector3(0, 0.75f, 0);
                _collider.height = 1.5f;
            }
            else
            {
                _collider.center = new Vector3(0, 1, 0);
                _collider.height = 2f;
            } 
        }
    }
}