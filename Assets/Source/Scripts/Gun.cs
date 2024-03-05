using System;
using UnityEngine;

namespace Source.Scripts
{
    public abstract class Gun : MonoBehaviour
    {
        [SerializeField] protected Bullet BulletPrefab;
        
        public Action ShootHappened;
    }
}