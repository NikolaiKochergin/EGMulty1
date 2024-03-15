using System.Collections.Generic;
using Colyseus;
using Unity.Mathematics;
using UnityEngine;

namespace Source.Scripts.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        private const string RoomName = "state_handler";

        [SerializeField] private LossCounter _lossCounter;
        [SerializeField] private PlayerCharacter _player;
        [SerializeField] private RemoteInput _enemy;
        [SerializeField] private Vector3[] _spawnPoints;

        private Dictionary<string, RemoteInput> _enemies = new Dictionary<string, RemoteInput>();
        private ColyseusRoom<State> _room;

        public LossCounter LossCounter => _lossCounter;

        protected override void Awake()
        {
            base.Awake();
            
            Instance.InitializeClient();
            Connect();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _room.Leave();
        }

        public void SendMessage(string key, Dictionary<string, object> data) => 
            _room.Send(key, data);

        public void SendMessage(string key, string data) => 
            _room.Send(key, data);

        public string GetSessionID() => 
            _room.SessionId;

        private async void Connect()
        {
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "speed", _player.Speed },
                { "hp", _player.MaxHealth },
                { "sp", _spawnPoints },
            };
            
            _room = await Instance.client.JoinOrCreate<State>(RoomName, data);

            _room.OnStateChange += OnStateChange;
            
            _room.OnMessage<string>(MessageName.Type.Shoot, ApplyShoot);
            _room.OnMessage<string>(MessageName.Type.Weapon, ApplyChangeWeapon);
        }

        private void OnStateChange(State state, bool isfirststate)
        {
            _room.OnStateChange -= OnStateChange;
            
            state.players.ForEach((key, player) =>
            {
                if(key == _room.SessionId)
                    CreatePlayer(player);
                else
                    CreateEnemy(key, player);
            });

            _room.State.players.OnAdd += CreateEnemy;
            _room.State.players.OnRemove += RemoveEnemy;
        }

        private void ApplyShoot(string jsonShootInfo)
        {
            ShootInfo shootInfo = JsonUtility.FromJson<ShootInfo>(jsonShootInfo);

            if (_enemies.ContainsKey(shootInfo.key) == false)
            {
                Debug.LogError($"Enemy with id: {shootInfo.key} does not exist.");
                return;
            }
            
            _enemies[shootInfo.key].Shoot(shootInfo);
        }

        private void ApplyChangeWeapon(string jsonChangeWeaponInfo)
        {
            ChangeWeaponInfo changeWeaponInfo = JsonUtility.FromJson<ChangeWeaponInfo>(jsonChangeWeaponInfo);
            if (_enemies.ContainsKey(changeWeaponInfo.key) == false)
            {
                Debug.LogError($"Enemy with id: {changeWeaponInfo.key} does not exist.");
                return;
            }

            _enemies[changeWeaponInfo.key].SetWeapon(changeWeaponInfo.id);
        }

        private void CreatePlayer(Player player)
        {
            Vector3 position = new Vector3(player.pX, player.pY, player.pZ);

            PlayerCharacter playerCharacter = Instantiate(_player, position, quaternion.identity);
            player.OnChange += playerCharacter.OnChange;
            
            _room.OnMessage<string>(MessageName.Type.Restart, playerCharacter.GetComponent<LocalInput>().Restart);
        }

        private void CreateEnemy(string key, Player player)
        {
            Vector3 position = new Vector3(player.pX, player.pY, player.pZ);
            RemoteInput enemy = Instantiate(_enemy, position, quaternion.identity);
            enemy.Init(key, player);
            
            _enemies.Add(key, enemy);
        }

        private void RemoveEnemy(string key, Player player)
        {
            if(_enemies.ContainsKey(key) == false)
                return;
            
            RemoteInput enemy = _enemies[key];
            enemy.Destroy();

            _enemies.Remove(key);
        }
    }
}