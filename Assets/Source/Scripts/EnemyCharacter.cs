using UnityEngine;

namespace Source.Scripts
{
    public class EnemyCharacter : MonoBehaviour
    {
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

        public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval)
        {
            TargetPosition = position + velocity * averageInterval;
            _velocityMagnitude = velocity.magnitude;
        }
    }
}