using System.Collections.Generic;
using Source.Scripts.Multiplayer;
using UnityEngine;

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
            SendMove();
        }

        private void SendMove()
        {
            _player.GetMoveInfo(out Vector3 position, out Vector3 velocity);
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"pX", position.x},
                {"pY", position.y},
                {"pZ", position.z},
                {"vX", velocity.x},
                {"vY", velocity.y},
                {"vZ", velocity.z},
            };
            
            MultiplayerManager.Instance.SendMessage("move", data);
        }
    }
}