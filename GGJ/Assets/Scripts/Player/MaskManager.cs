using System;
using Masks;
using UnityEngine;

namespace Player
{
    public delegate void MaskEvent(MaskType oldType, MaskType newType);
    
    public class MaskManager : MonoBehaviour
    {
        public event MaskEvent OnMaskChanged;
        
        [HideInInspector] public MaskType currentMaskType = MaskType.DashMask;

        public void MaskPickedUp(MaskType maskType)
        {
            OnMaskChanged?.Invoke(currentMaskType, maskType);
            currentMaskType = maskType;
        }

        public void TryActivateCurrentMask()
        {
            switch (currentMaskType)
            {
                case MaskType.None:
                    break;
                case MaskType.DashMask:
                    Debug.Log("Dashing!");
                    GetComponent<Dash>().TryDash(transform.forward);
                    break;
                case MaskType.ShrinkMask:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}