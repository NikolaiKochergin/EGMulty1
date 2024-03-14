using System.Collections.Generic;
using Colyseus.Schema;
using Source.Scripts.Multiplayer;
using UnityEngine;

namespace Source.Scripts
{
    public class PlayerCharacter : Character
    {
        [SerializeField] private HealthView _healthView;
        [SerializeField] private Transform _head;
        [SerializeField] private Transform _cameraPoint;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private CheckFly _checkFly;
        [SerializeField] private float _maxHeadAngle = 90f;
        [SerializeField] private float _minHeadAngle = -90f;
        [SerializeField] private float _jumpForce = 6f;
        [SerializeField] private float _jumpDelay = 0.2f;

        private Health _health;
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
            _health = new Health(MaxHealth);
            _healthView.Initialize(_health);
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

        public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY)
        {
            position = transform.position;
            velocity = _rigidbody.velocity;
            rotateX = _head.localEulerAngles.x;
            rotateY = transform.eulerAngles.y;
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

        public void OnChange(List<DataChange> changes)
        {
            foreach (DataChange dataChange in changes)
            {
                switch (dataChange.Field)
                {
                    case "loss":
                        MultiplayerManager.Instance.LossCounter.SetPlayerLoss((byte)dataChange.Value);
                        break;
                    case "currentHP":
                        _health.SetCurrentHP((sbyte)dataChange.Value);
                        break;
                }
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
