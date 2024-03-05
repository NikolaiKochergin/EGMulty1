using UnityEngine;

namespace Source.Scripts
{
    public class GunAnimator : MonoBehaviour
    {
        private static readonly int Shoot = Animator.StringToHash("Shoot");
        
        [SerializeField] private PlayerGun _gun;
        [SerializeField] private Animator _animator;

        private void Start() => 
            _gun.ShootHappened += OnShootHappened;

        private void OnDestroy() => 
            _gun.ShootHappened -= OnShootHappened;

        private void OnShootHappened() => 
            _animator.SetTrigger(Shoot);
    }
}
