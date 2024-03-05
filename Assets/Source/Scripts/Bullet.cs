using System.Collections;
using UnityEngine;

namespace Source.Scripts
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _lifeTime = 5f;

        public void Init(Vector3 velocity)
        {
            _rigidbody.velocity = velocity;
            StartCoroutine(DelayDestroy());
        }

        private void OnCollisionEnter(Collision other) => 
            Destroy();

        private IEnumerator DelayDestroy()
        {
            yield return new WaitForSeconds(_lifeTime);
            Destroy();
        }

        private void Destroy() => 
            Destroy(gameObject);
    }
}
