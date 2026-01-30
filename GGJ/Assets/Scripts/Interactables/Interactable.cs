using Masks;
using UnityEngine;

namespace Interaction
{
    public enum InteractionType
    {
        Use,
        Pickup,
    }
    
    public abstract class Interactable : MonoBehaviour
    {
        public virtual InteractionType GetInteractionType() => InteractionType.Pickup;
        public virtual void Interact() {}
        public virtual void Consume() => Destroy(gameObject);
    }
}
