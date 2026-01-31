using UnityEngine;

namespace Player
{
    public delegate void PlayerEvent();
    public class Player : MonoBehaviour
    {
        //Any player-specific events to bind to
        public event PlayerEvent MaskInputPressed;
        public event PlayerEvent MaskInputHeld;
        public event PlayerEvent MaskInputReleased;

        [HideInInspector] public Vector2 LookInput;
    }
}
