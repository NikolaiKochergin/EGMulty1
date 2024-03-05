using UnityEngine;

namespace Source.Scripts
{
    public class EnemyGun : Gun
    {

        public void Shoot(Vector3 position, Vector3 velocity)
        {
            Instantiate(BulletPrefab, position, Quaternion.identity)
                .Init(velocity);
            
            ShootHappened?.Invoke();
        }
    }
}
