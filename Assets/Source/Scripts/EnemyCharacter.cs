using UnityEngine;

namespace Source.Scripts
{
    public class EnemyCharacter : MonoBehaviour
    {
        private Vector3 _targetPosition;
        private float _speed;
        private float _lastCallTime;

        public void SetPosition(Vector3 position)
        {
            _targetPosition = position;

            float distance = Vector3.Distance(transform.position, position);

            float currentTime = Time.time;
            float deltaTime = currentTime - _lastCallTime;
            _lastCallTime = currentTime;

            _speed = distance / deltaTime;
        }

        private void Update() => 
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

    }
}
