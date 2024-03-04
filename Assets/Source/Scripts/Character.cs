using UnityEngine;

namespace Source.Scripts
{
    public class Character : MonoBehaviour
    {
        [field: SerializeField] public float Speed { get; protected set; } = 5f;
        public Vector3 Velocity { get; protected set; }
    }
}