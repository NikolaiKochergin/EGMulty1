using System.Collections.Generic;
using Colyseus;
using Unity.Mathematics;
using UnityEngine;

namespace Source.Scripts.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        [SerializeField] private GameObject _player;
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

        private async void Connect()
        {
            _room = await Instance.client.JoinOrCreate<State>("state_handler");

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
            Vector3 position = new Vector3(player.x, 0, player.y);
            Instantiate(_player, position, quaternion.identity);
        }

        private void CreateEnemy(string key, Player player)
        {
            Vector3 position = new Vector3(player.x, 0, player.y);
            RemoteInput enemy = Instantiate(_enemy, position, quaternion.identity);

            player.OnChange += enemy.OnChange;
        }

        private void RemoveEnemy(string key, Player player)
        {
        }
    }
}