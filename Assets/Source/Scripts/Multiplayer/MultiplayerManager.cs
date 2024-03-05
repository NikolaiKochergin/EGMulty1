using System.Collections.Generic;
using Colyseus;
using Unity.Mathematics;
using UnityEngine;

namespace Source.Scripts.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        [SerializeField] private PlayerCharacter _player;
        [SerializeField] private RemoteInput _enemy;
        
        private ColyseusRoom<State> _room;
        
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
                { "speed", _player.Speed }
            };
            
            _room = await Instance.client.JoinOrCreate<State>("state_handler", data);

            _room.OnStateChange += OnStateChange;
        }

        private void OnStateChange(State state, bool isfirststate)
        {
            if(isfirststate == false)
                return;
            
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

        private void CreatePlayer(Player player)
        {
            Vector3 position = new Vector3(player.pX, player.pY, player.pZ);
            Instantiate(_player, position, quaternion.identity);
        }

        private void CreateEnemy(string key, Player player)
        {
            Vector3 position = new Vector3(player.pX, player.pY, player.pZ);
            RemoteInput enemy = Instantiate(_enemy, position, quaternion.identity);
            enemy.Init(player);
        }

        private void RemoveEnemy(string key, Player player)
        {
        }
    }
}