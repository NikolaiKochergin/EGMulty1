using UnityEngine;

namespace Source.Scripts
{
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed = 5f;

        private Vector3 _direction;

        public void SetInput(float h, float v)
        {
            _direction.x = h;
            _direction.z = v;
        }

        public void GetMoveInfo(out Vector3 position, out Vector3 velocity)
        {
            position = transform.position;
            velocity = _rigidbody.velocity;
        }

        private void Update() => 
            Move();

        private void Move()
        {
            Vector3 velocity = (transform.forward * _direction.z + transform.right * _direction.x).normalized * _speed;
            _rigidbody.velocity = velocity;
        }
    }
}
