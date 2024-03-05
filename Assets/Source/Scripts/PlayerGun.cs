using System;
using UnityEngine;

namespace Source.Scripts
{
    public class PlayerGun : MonoBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _shootDelay = 0.2f;

        private float _lastShootTime;

        public event Action ShootHappened;

        public bool TryShoot(out ShootInfo info)
        {
            info = new ShootInfo();
            
            if(Time.time - _lastShootTime < _shootDelay)
                return false;

            Vector3 position = _shootPoint.position;
            Vector3 direction = _shootPoint.forward;
            _lastShootTime = Time.time;
            
            Instantiate(_bulletPrefab, _shootPoint.position, _shootPoint.rotation)
                .Init(_shootPoint.forward, _bulletSpeed);

            direction *= _bulletSpeed;
            info.pX = position.x;
            info.pY = position.y;
            info.pZ = position.z;
            info.dX = direction.x;
            info.dY = direction.y;
            info.dZ = direction.z;
            
            ShootHappened?.Invoke();
            return true;
        }
    }
}
