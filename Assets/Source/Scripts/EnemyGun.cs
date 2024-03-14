using UnityEngine;

namespace Source.Scripts
{
    public class EnemyGun : Gun
    {
        [SerializeField] private GameObject _gunView;
        
        public void Shoot(Vector3 position, Vector3 velocity)
        {
            Instantiate(BulletPrefab, position, Quaternion.identity)
                .Init(velocity);
            
            ShootHappened?.Invoke();
        }

        public void SetActive(bool value) => 
            _gunView.SetActive(value);
    }
}
