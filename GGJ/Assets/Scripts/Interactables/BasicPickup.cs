using Interaction;
using UnityEngine;

namespace Interactables
{
    public class BasicPickup : Interactable
    {
        [HideInInspector] public Throw currentParent = null;
        public override InteractionType GetInteractionType() => InteractionType.Pickup;
        public override void Consume() {}
    }
}
