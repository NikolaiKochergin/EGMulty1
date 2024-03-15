using System;
using System.Collections;
using System.Collections.Generic;
using Source.Scripts.Multiplayer;
using UnityEngine;

namespace Source.Scripts
{
    public class LocalInput : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter _player;
        [SerializeField] private PlayerGun[] _guns;
        [SerializeField] private float _mouseSensitivity = 2f;
        [SerializeField] private float _restartDelay = 3f;

        private MultiplayerManager _multiplayerManager;
        private bool _hold = false;
        private PlayerGun _currentGun;

        private void Start()
        {
            _multiplayerManager = MultiplayerManager.Instance;
            SetWeapon(WeaponId.Gun);
        }

        private void Update()
        {
            if(_hold)
                return;
            
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            bool isShoot = Input.GetMouseButton(0);

            bool space = Input.GetKeyDown(KeyCode.Space);

            _player.SetInput(h, v, mouseX * _mouseSensitivity);
            _player.RotateX(-mouseY * _mouseSensitivity);
            
            if(space)
                _player.Jump();
            
            if(isShoot && _currentGun.TryShoot(out ShootInfo shootInfo)) 
                SendShoot(ref shootInfo);

            if (Input.GetKeyDown(KeyCode.Alpha1))
                SetWeapon(WeaponId.Gun);
                
            if(Input.GetKeyDown(KeyCode.Alpha2))
                SetWeapon(WeaponId.Shotgun);
            
            SendMove();
        }

        private void SetWeapon(WeaponId id)
        {
            foreach (PlayerGun gun in _guns)
            {
                if (gun.Id == id)
                {
                    gun.SetActive(true);
                    _currentGun = gun;
                    ChangeWeaponInfo data = new ChangeWeaponInfo
                    {
                        key = _multiplayerManager.GetSessionID(),
                        id = id,
                    };
                    string json = JsonUtility.ToJson(data);
                    _multiplayerManager.SendMessage(MessageName.Key.weapon, json);
                }
                else
                {
                    gun.SetActive(false);
                }
            }
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
            
            _multiplayerManager.SendMessage(MessageName.Key.move, data);
        }

        private void SendShoot(ref ShootInfo shootInfo)
        {
            shootInfo.key = _multiplayerManager.GetSessionID();
            string json = JsonUtility.ToJson(shootInfo);
            _multiplayerManager.SendMessage(MessageName.Key.shoot, json);
        }

        public void Restart(string jsonRestartInfo)
        {
            RestartInfo info = JsonUtility.FromJson<RestartInfo>(jsonRestartInfo);
            StartCoroutine(Hold());
            
            _player.transform.position = new Vector3(info.x, 0, info.z);
            _player.SetInput(0,0,0);
            
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"pX", info.x},
                {"pY", 0},
                {"pZ", info.z},
                {"vX", 0},
                {"vY", 0},
                {"vZ", 0},
                {"rX", 0},
                {"rY", 0},
            };
            _multiplayerManager.SendMessage(MessageName.Key.move, data);
        }

        private IEnumerator Hold()
        {
            _hold = true;
            yield return new WaitForSecondsRealtime(_restartDelay);
            _hold = false;
        }
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

    [Serializable]
    public struct RestartInfo
    {
        public float x;
        public float z;
    }

    [Serializable]
    public struct ChangeWeaponInfo
    {
        public string key;
        public WeaponId id;
    }
}