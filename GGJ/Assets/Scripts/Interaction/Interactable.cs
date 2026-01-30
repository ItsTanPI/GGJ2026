using UnityEngine;

namespace Interaction
{
    public enum InteractionType
    {
        Use,
        Pickup,
    }
    
    public interface IInteractable<out T>
    {
        public InteractionType GetInteractionType();
        public T Interact();
    }
}
