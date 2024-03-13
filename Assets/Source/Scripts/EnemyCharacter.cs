using System.Collections.Generic;
using Source.Scripts.Multiplayer;
using UnityEngine;

namespace Source.Scripts
{
    public class EnemyCharacter : Character
    {
        [SerializeField] private Transform _head;
        [SerializeField] private HealthView _healthView;
        
        private Health _health;
        private float _velocityMagnitude = 0f;
        private string _sessionID;

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

        public void Init(string sessionID, float speed, int maxHP)
        {
            _sessionID = sessionID;
            SetSpeed(speed);
            SetMaxHP(maxHP);
        }

        public void ApplyDamage(int damage)
        {
            _health.ApplyDamage(damage);

            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "id", _sessionID },
                { "value", damage},
            };
            
            MultiplayerManager.Instance.SendMessage(MessageName.Key.damage, data);
        }

        public void RestoreHP(int newValue) => 
            _health.SetCurrentHP(newValue);

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

        private void SetSpeed(float value) => 
            Speed = value;

        private void SetMaxHP(int value)
        {
            _health = new Health(value);
            _healthView.Initialize(_health);
        }
    }
}