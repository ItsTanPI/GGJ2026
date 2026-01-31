using Interaction;
using Masks;
using UnityEngine;

namespace Interactables
{
    public class MaskPickup : Interactable
    {
        [SerializeField] private MaskType maskType = MaskType.DashMask;
        public MaskType GetMaskType() => maskType;
    }
}