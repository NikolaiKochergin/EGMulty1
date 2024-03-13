using TMPro;
using UnityEngine;

namespace Source.Scripts
{
    public class LossCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private int _enemyLoss;
        private int _playerLoss;

        public void SetEnemyLoss(int value)
        {
            _enemyLoss = value;
            UpdateText();
        }

        public void SetPlayerLoss(int value)
        {
            _playerLoss = value;
            UpdateText();
        }

        private void UpdateText() => 
            _text.SetText($"{_playerLoss} : {_enemyLoss}");
    }
}
