using Colyseus;
using Unity.Mathematics;
using UnityEngine;

namespace Source.Scripts.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        [SerializeField] private GameObject _player;
        [SerializeField] private GameObject _enemy;
        
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

        private async void Connect()
        {
            _room = await Instance.client.JoinOrCreate<State>("state_handler");

            _room.OnStateChange += OnStateChange;
        }

        private void OnStateChange(State state, bool isfirststate)
        {
            if(isfirststate == false)
                return;

            Player player = state.players[_room.SessionId];
            Vector3 position = new Vector3(player.x - 200, 0, player.y- 200) / 8;
            Instantiate(_player, position, quaternion.identity);
            
            state.players.ForEach(ForEachEnemy);
        }

        private void ForEachEnemy(string key, Player player)
        {
            if(key == _room.SessionId)
                return;
            
            Vector3 position = new Vector3(player.x - 200, 0, player.y- 200) / 8;
            Instantiate(_enemy, position, quaternion.identity);
        }
    }
}