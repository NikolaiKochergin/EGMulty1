using UnityEngine;

namespace Source.Scripts
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private HealthBar _healthBar;
        
        private Health _health;

        public void Initialize(Health health)
        {
            _health = health;
            DisplayHealth();
            health.Changed += DisplayHealth;
        }

        private void DisplayHealth() => 
            _healthBar.SetFill((float)_health.CurrentValue / _health.MaxValue);
    }
}