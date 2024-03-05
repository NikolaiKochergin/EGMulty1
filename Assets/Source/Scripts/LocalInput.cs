using System;
using System.Collections.Generic;
using Source.Scripts.Multiplayer;
using UnityEngine;

namespace Source.Scripts
{
    public class LocalInput : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter _player;
        [SerializeField] private PlayerGun _gun;
        [SerializeField] private float _mouseSensitivity = 2f;

        private MultiplayerManager _multiplayerManager;

        private void Start() => 
            _multiplayerManager = MultiplayerManager.Instance;

        private void Update()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            bool isShoot = Input.GetMouseButton(0);

            bool space = Input.GetKeyDown(KeyCode.Space);

            bool isCrouch = Input.GetKey(KeyCode.LeftControl);

            _player.SetInput(h, v, mouseX * _mouseSensitivity);
            _player.RotateX(-mouseY * _mouseSensitivity);
            
            if(space)
                _player.Jump();

            if (isCrouch != _player.IsCrouch)
            {
                _player.SetCrouch(isCrouch);
                SendCrouch(isCrouch);
            }
            
            if(isShoot && _gun.TryShoot(out ShootInfo shootInfo)) 
                SendShoot(ref shootInfo);
            
            SendMove();
        }

        private void SendCrouch(bool isCrouch)
        {
            CrouchInfo crouchInfo = new CrouchInfo()
            {
                key = _multiplayerManager.GetSessionID(),
                isCrch = isCrouch,
            };

            string json = JsonUtility.ToJson(crouchInfo);
            
            _multiplayerManager.SendMessage("crouch", json);
        }

        private void SendMove()
        {
            _player.GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY);
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"pX", position.x},
                {"pY", position.y},
                {"pZ", position.z},
                {"vX", velocity.x},
                {"vY", velocity.y},
                {"vZ", velocity.z},
                {"rX", rotateX},
                {"rY", rotateY},
            };
            
            _multiplayerManager.SendMessage("move", data);
        }

        private void SendShoot(ref ShootInfo shootInfo)
        {
            shootInfo.key = _multiplayerManager.GetSessionID();
            string json = JsonUtility.ToJson(shootInfo);
            _multiplayerManager.SendMessage("shoot", json);
        }
    }

    [Serializable]
    public struct CrouchInfo
    {
        public string key;
        public bool isCrch;
    }

    [Serializable]
    public struct ShootInfo
    {
        public string key;
        public float pX;
        public float pY;
        public float pZ;
        public float dX;
        public float dY;
        public float dZ;
    }
}