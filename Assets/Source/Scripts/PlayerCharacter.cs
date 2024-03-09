using UnityEngine;

namespace Source.Scripts
{
    public class PlayerCharacter : Character
    {
        [SerializeField] private Transform _head;
        [SerializeField] private Transform _cameraPoint;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private CapsuleCollider _collider;
        [SerializeField] private CheckFly _checkFly;
        [SerializeField] private float _maxHeadAngle = 90f;
        [SerializeField] private float _minHeadAngle = -90f;
        [SerializeField] private float _jumpForce = 6f;
        [SerializeField] private float _jumpDelay = 0.2f;

        private Vector3 _direction;
        private float _rotateY;
        private float _currentRotateX;
        private float _jumpTime;

        private void Start()
        {
            Transform camera = Camera.main.transform;
            camera.parent = _cameraPoint;
            camera.localPosition = Vector3.zero;
            camera.localRotation = Quaternion.identity;
        }

        private void FixedUpdate()
        {
            Move();
            RotateY();
        }

        public void SetInput(float h, float v, float rotateY)
        {
            _direction.x = h;
            _direction.z = v;
            _rotateY += rotateY;
        }

        public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out Vector2 rotation)
        {
            position = transform.position;
            velocity = _rigidbody.velocity;
            rotation.x = _head.localEulerAngles.x;
            rotation.y = transform.eulerAngles.y;
        }

        public void RotateX(float value)
        {
            _currentRotateX = Mathf.Clamp(_currentRotateX + value, _minHeadAngle, _maxHeadAngle);
            _head.localEulerAngles = new Vector3(_currentRotateX, 0, 0);
        }

        public void Jump()
        {
            if(_checkFly.IsFly)
                return;
            
            if(Time.time - _jumpTime < _jumpDelay)
                return;

            _jumpTime = Time.time;
            
            _rigidbody.AddForce(0,_jumpForce,0, ForceMode.VelocityChange);
        }

        public void SetCrouch(bool value)
        {
            IsCrouch = value;
            if (value)
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

        private void Move()
        {
            Vector3 velocity = (transform.forward * _direction.z + transform.right * _direction.x).normalized * Speed;
            velocity.y = _rigidbody.velocity.y;
            Velocity = velocity;
            _rigidbody.velocity = Velocity;
        }

        private void RotateY()
        {
            _rigidbody.angularVelocity = new Vector3(0, _rotateY, 0);
            _rotateY = 0;
        }
    }
}
