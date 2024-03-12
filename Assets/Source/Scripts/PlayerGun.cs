using UnityEngine;

namespace Source.Scripts
{
    public class PlayerGun : Gun
    {
        [SerializeField] private int _damage = 1;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _shootDelay = 0.2f;

        private float _lastShootTime;

        public bool TryShoot(out ShootInfo info)
        {
            info = new ShootInfo();
            
            if(Time.time - _lastShootTime < _shootDelay)
                return false;

            Vector3 position = _shootPoint.position;
            Vector3 velocity = _shootPoint.forward * _bulletSpeed;
            _lastShootTime = Time.time;
            
            Instantiate(BulletPrefab, _shootPoint.position, _shootPoint.rotation)
                .Init(velocity, _damage);

            info.pX = position.x;
            info.pY = position.y;
            info.pZ = position.z;
            info.dX = velocity.x;
            info.dY = velocity.y;
            info.dZ = velocity.z;
            
            ShootHappened?.Invoke();
            return true;
        }
    }
}
