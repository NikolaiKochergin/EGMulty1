using System;
using UnityEngine;

namespace Source.Scripts
{
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private Transform _head;
        [SerializeField] private Transform _cameraPoint;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _maxHeadAngle = 90f;
        [SerializeField] private float _minHeadAngle = -90f;
        [SerializeField] private float _jumpForce = 6f;

        private Vector3 _direction;
        private float _rotateY;
        private float _currentRotateX;
        private bool _isFly = true;

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

        private void OnCollisionStay(Collision other)
        {
            ContactPoint[] contactPoint = other.contacts;
            foreach (ContactPoint point in contactPoint)
            {
                if (point.normal.y > 0.45f)
                    _isFly = false;
            }
        }

        private void OnCollisionExit(Collision other) => 
            _isFly = true;

        public void SetInput(float h, float v, float rotateY)
        {
            _direction.x = h;
            _direction.z = v;
            _rotateY += rotateY;
        }

        public void GetMoveInfo(out Vector3 position, out Vector3 velocity)
        {
            position = transform.position;
            velocity = _rigidbody.velocity;
        }

        public void RotateX(float value)
        {
            _currentRotateX = Mathf.Clamp(_currentRotateX + value, _minHeadAngle, _maxHeadAngle);
            _head.localEulerAngles = new Vector3(_currentRotateX, 0, 0);
        }

        public void Jump()
        {
            if(_isFly)
                return;
            
            _rigidbody.AddForce(0,_jumpForce,0, ForceMode.VelocityChange);
        }

        private void Move()
        {
            Vector3 velocity = (transform.forward * _direction.z + transform.right * _direction.x).normalized * _speed;
            velocity.y = _rigidbody.velocity.y;
            _rigidbody.velocity = velocity;
        }

        private void RotateY()
        {
            _rigidbody.angularVelocity = new Vector3(0, _rotateY, 0);
            _rotateY = 0;
        }
    }
}
