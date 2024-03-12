using UnityEngine;

namespace Source.Scripts
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private RectTransform _fill;

        private float _defaultWidth;

        private void Awake() => 
            _defaultWidth = _fill.sizeDelta.x;

        public void SetFill(float value)
        {
            Debug.LogWarning("Health value " + value );
            _fill.sizeDelta = new Vector2(_defaultWidth * value, _fill.sizeDelta.y);
        }
    }
}
