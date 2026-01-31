using Masks;
using UnityEngine;

namespace Interaction
{
    public enum InteractionType
    {
        None,
        Use,
        Pickup,
        Mask
    }
    
    public abstract class Interactable : MonoBehaviour
    {
        public virtual InteractionType GetInteractionType() => InteractionType.None;
        public virtual void Interact() {}
        public virtual void Consume() {}
    }
}
