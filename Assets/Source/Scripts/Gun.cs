using System;
using UnityEngine;

namespace Source.Scripts
{
    public abstract class Gun : MonoBehaviour
    {
        [SerializeField] protected Bullet BulletPrefab;
        [SerializeField] private WeaponId _id;

        public WeaponId Id => _id;
        
        public Action ShootHappened;
    }

    public enum WeaponId
    {
        Gun = 0,
        Shotgun = 1,
    }
}