using Core.Extensions;
using Core.Input;
using UnityEngine;

namespace Bremsengine
{
    public class PlayerVelocityRotator : MonoBehaviour
    {
        [SerializeField] Transform rotationAnchor;
        [SerializeField] float weight = 3f;
        [SerializeField] float idleMultiplier = 2.5f;
        [SerializeField] Quaternion leftAngle;
        [SerializeField] Quaternion rightAngle;
        private void Update()
        {
            Vector2 input = PlayerInputController.actions.Player.Move.ReadValue<Vector2>();

            Quaternion target = input.x == 0f ? Quaternion.identity : (input.x.Sign() > 0f ? rightAngle : leftAngle);
            bool idle = input.x == 0f;
            rotationAnchor.rotation = Quaternion.Lerp(rotationAnchor.rotation, target, Time.deltaTime * weight * (idle ? idleMultiplier : 1f));
        }
    }
}
