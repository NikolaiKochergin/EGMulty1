using System.Collections.Generic;
using System.Linq;
using Colyseus.Schema;
using Source.Scripts.Multiplayer;
using UnityEngine;

namespace Source.Scripts
{
    public class RemoteInput : MonoBehaviour
    {
        [SerializeField] private EnemyCharacter _character;
        [SerializeField] private EnemyGun[] _guns;

        private readonly List<float> _receiveTimeInterval = new List<float>{0, 0, 0, 0, 0};
        private float _lastReceiveTime = 0f;
        private Player _player;
        private EnemyGun _currentGun;

        private float AverageInterval => _receiveTimeInterval.Sum() / _receiveTimeInterval.Count;

        public void Init(string key, Player player)
        {
            _player = player;
            _character.Init(key, player.speed, player.maxHP);
            player.OnChange += OnChange;
            SetWeapon(WeaponId.Gun);
        }

        public void Shoot(in ShootInfo info)
        {
            Vector3 position = new Vector3(info.pX, info.pY, info.pZ);
            Vector3 velocity = new Vector3(info.dX, info.dY, info.dZ);
            
            _currentGun.Shoot(position, velocity);
        }

        public void SetWeapon(WeaponId id)
        {
            foreach (EnemyGun gun in _guns)
            {
                if (gun.Id == id)
                {
                    gun.SetActive(true);
                    _currentGun = gun;
                }
                else
                {
                    gun.SetActive(false);
                }
            }
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
                    case "loss":
                        MultiplayerManager.Instance.LossCounter.SetEnemyLoss((byte)dataChange.Value);
                        break;
                    case "currentHP":
                        if((sbyte)dataChange.Value > (sbyte)dataChange.PreviousValue)
                            _character.RestoreHP((sbyte)dataChange.Value);
                        break;
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