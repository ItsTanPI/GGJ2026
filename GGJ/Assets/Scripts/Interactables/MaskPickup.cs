using Interaction;
using Masks;
using UnityEngine;

namespace Interactables
{
    public class MaskPickup : Interactable
    {
        [SerializeField] private MaskType maskType = MaskType.DashMask;
        [HideInInspector] public Throw currentParent = null;
        
        public MaskType GetMaskType() => maskType;

        //Mask remains alive with player
        public override void Consume()
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        public void Throw()
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}