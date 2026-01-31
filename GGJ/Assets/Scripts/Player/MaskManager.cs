using System;
using Masks;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public delegate void MaskEvent(MaskType oldType, MaskType newType);
    
    public class MaskManager : MonoBehaviour
    {
        public event MaskEvent OnMaskChanged;
        
        public MaskType currentMaskType = MaskType.ShrinkMask;

        public void MaskPickedUp(MaskType maskType)
        {
            OnMaskChanged?.Invoke(currentMaskType, maskType);
            currentMaskType = maskType;
        }

        public void MaskDropped(MaskType maskType)
        {
            OnMaskChanged?.Invoke(currentMaskType, MaskType.None);

            switch (currentMaskType)
            {
                case MaskType.None:
                    break;
                case MaskType.DashMask:
                    GetComponent<Dash>().RevertDash();
                    break;
                case MaskType.ShrinkMask:
                    GetComponent<Shrink>().RevertShrink();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            currentMaskType = MaskType.None;
        }
        
        public void TryActivateCurrentMask()
        {
            Debug.Log("Attempting to activate the mask: " + currentMaskType.ToString());
            
            switch (currentMaskType)
            {
                case MaskType.None:
                    break;
                case MaskType.DashMask:
                    GetComponent<Dash>().TryDash(transform.forward);
                    break;
                case MaskType.ShrinkMask:
                    GetComponent<Shrink>().TryToggleShrink();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}