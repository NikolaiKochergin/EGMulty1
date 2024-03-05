using UnityEngine;

namespace Source.Scripts
{
    public class CharacterAnimator : MonoBehaviour
    {
        private static readonly int Grounded = Animator.StringToHash("Grounded");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Crouch = Animator.StringToHash("Crouch");

        [SerializeField] private Animator _bodyAnimator;
        [SerializeField] private Animator _footAnimator;
        [SerializeField] private CheckFly _checkFly;
        [SerializeField] private Character _character;

        private void Update()
        {
            Vector3 localVelocity = _character.transform.InverseTransformVector(_character.Velocity);
            float speed = localVelocity.magnitude / _character.Speed;
            float sign = Mathf.Sign(localVelocity.z);
            
            _footAnimator.SetFloat(Speed, speed * sign);
            _footAnimator.SetBool(Grounded, _checkFly.IsFly == false);
            _bodyAnimator.SetBool(Crouch, _character.IsCrouch);
        }
    }
}
