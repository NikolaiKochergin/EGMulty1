using UnityEngine;

namespace Source.Scripts
{
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private Vector3 _direction;

        public void SetInput(float h, float v)
        {
            _direction.x = h;
            _direction.z = v;
        }

        public void GetMoveInfo(out Vector3 position) => 
            position = transform.position;

        private void Update() => 
            Move();

        private void Move() => 
            transform.position += _direction * (_speed * Time.deltaTime);
    }
}
