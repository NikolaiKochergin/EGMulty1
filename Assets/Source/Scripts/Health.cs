using System;

namespace Source.Scripts
{
    public class Health
    {
        public Health(int maxValue)
        {
            MaxValue = maxValue;
            CurrentValue = maxValue;
        }
    
        public int MaxValue { get; }
        public int CurrentValue { get; private set; }

        public event Action Changed;
        public event Action Died;
    
        public void ApplyDamage(int value)
        {
            if (CurrentValue <= 0 || value <= 0)
                return;
            
            if (CurrentValue - value <= 0)
            {
                CurrentValue = 0;
                Changed?.Invoke();
                Died?.Invoke();
            }
            else
            {
                CurrentValue -= value;
                Changed?.Invoke();
            }
        }

        public void SetCurrentHP(int newValue)
        {
            CurrentValue = newValue > MaxValue
                ? MaxValue
                : newValue;
            
            Changed?.Invoke();
        }
    }
}