using UnityEngine;
using UnityEngine.Serialization;

namespace Source.Scripts
{
    public class LocalInput : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter _player;

        private void Update()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            _player.SetInput(h, v);
        }
    }
}