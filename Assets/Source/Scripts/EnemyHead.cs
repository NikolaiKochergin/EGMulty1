using UnityEngine;

namespace Source.Scripts
{
    public class EnemyHead : MonoBehaviour , IDamageable
    {
        [SerializeField] private EnemyCharacter _enemyCharacter;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private int _damageMultiplayer;
        
        public void ApplyDamage(int damage)
        {
            damage *= _damageMultiplayer;
            _enemyCharacter.ApplyDamage(damage);
            _particleSystem.Play();
        }
    }
}
