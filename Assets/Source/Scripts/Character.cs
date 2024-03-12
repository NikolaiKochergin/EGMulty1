using UnityEngine;

namespace Source.Scripts
{
    public abstract class Character : MonoBehaviour
    {
        [field: SerializeField] public int MaxHealth { get; protected set; } = 10;
        [field: SerializeField] public float Speed { get; protected set; } = 5f;
        public Vector3 Velocity { get; protected set; }
    }
}