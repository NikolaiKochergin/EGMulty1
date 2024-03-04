using UnityEngine;

namespace Source.Scripts
{
    public class EnemyCharacter : Character
    {
        [SerializeField] private Transform _head;
        
        private float _velocityMagnitude = 0f;
        
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