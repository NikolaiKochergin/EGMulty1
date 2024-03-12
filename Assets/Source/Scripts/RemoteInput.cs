using System.Collections.Generic;
using System.Linq;
using Colyseus.Schema;
using UnityEngine;

namespace Source.Scripts
{
    public class RemoteInput : MonoBehaviour
    {
        [SerializeField] private EnemyCharacter _character;
        [SerializeField] private EnemyGun _gun;

        private readonly List<float> _receiveTimeInterval = new List<float>{0, 0, 0, 0, 0};
        private float _lastReceiveTime = 0f;
        private Player _player;

        private float AverageInterval => _receiveTimeInterval.Sum() / _receiveTimeInterval.Count;

        public void Init(Player player)
        {
            _player = player;
            _character.SetSpeed(player.speed);
            _character.SetMaxHP(player.hp);
            player.OnChange += OnChange;
        }

        public void Shoot(in ShootInfo info)
        {
            Vector3 position = new Vector3(info.pX, info.pY, info.pZ);
            Vector3 velocity = new Vector3(info.dX, info.dY, info.dZ);
            
            _gun.Shoot(position, velocity);
        }

        public void Destroy()
        {
            _player.OnChange -= OnChange;
            Destroy(gameObject);
        }

        private void OnChange(List<DataChange> changes)
        {
            SaveReceiveTime();
            
            Vector3 position = _character.TargetPosition;
            Vector3 velocity = _character.Velocity;
            foreach (DataChange dataChange in changes)
            {
                switch (dataChange.Field)
                {
                    case "pX":
                        position.x = (float) dataChange.Value;
                        break;
                    case "pY":
                        position.y = (float) dataChange.Value;
                        break;
                    case "pZ":
                        position.z = (float) dataChange.Value;
                        break;
                    case "vX":
                        velocity.x = (float) dataChange.Value;
                        break;
                    case "vY":
                        velocity.y = (float) dataChange.Value;
                        break;
                    case "vZ":
                        velocity.z = (float) dataChange.Value;
                        break;
                    case "rX":
                        _character.SetRotateX((float) dataChange.Value);
                        break;
                    case "rY":
                        _character.SetRotateY((float) dataChange.Value);
                        break;
                    default:
                        Debug.LogWarning($"Field {dataChange.Field} is not processed.");
                        break;
                }
            }

            _character.SetMovement(position, velocity, AverageInterval);
        }

        private void SaveReceiveTime()
        {
            float interval = Time.time - _lastReceiveTime;
            _lastReceiveTime = Time.time;
            
            _receiveTimeInterval.Add(interval);
            _receiveTimeInterval.RemoveAt(0);
        }
    }
}