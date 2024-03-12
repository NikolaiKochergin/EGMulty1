using UnityEngine;

namespace Source.Scripts
{
    public class EnemyCharacter : Character
    {
        [SerializeField] private Transform _head;
        [SerializeField] private HealthView _healthView;
        
        private float _velocityMagnitude = 0f;
        
        public Health Health { get; private set; }
        
        public Vector3 TargetPosition { get; private set; } = Vector3.zero;

        private void Start() => 
            TargetPosition = transform.position;

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

        public void SetMaxHP(int value)
        {
            Health = new Health(value);
            _healthView.Initialize(Health);
        }

        public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval)
        {
            TargetPosition = position + velocity * averageInterval;
            _velocityMagnitude = velocity.magnitude;
            
            Velocity = velocity;
        }

        public void SetRotateX(float value) => 
            _head.localEulerAngles = new Vector3(value, 0, 0);

        public void SetRotateY(float value) => 
            transform.localEulerAngles = new Vector3(0, value, 0);
    }
}