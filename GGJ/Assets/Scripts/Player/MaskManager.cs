using System;
using Masks;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public delegate void MaskEvent(MaskType oldType, MaskType newType);
    
    public class MaskManager : MonoBehaviour
    {
        
        public Animator animator;
        public event MaskEvent OnMaskChanged;
        
        private MaskType _currentMaskType = MaskType.None;

        public void MaskPickedUp(MaskType maskType)
        {
            Debug.Log("Mask Picked Up: " + maskType);
            OnMaskChanged?.Invoke(_currentMaskType, maskType);
            _currentMaskType = maskType;
        }

        public void MaskDropped(MaskType maskType)
        {
            OnMaskChanged?.Invoke(_currentMaskType, MaskType.None);

            switch (_currentMaskType)
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
            
            _currentMaskType = MaskType.None;
        }
        
        public void TryActivateCurrentMask()
        {
            animator?.SetTrigger("Resurrect");
            Debug.Log("Attempting to activate the mask: " + _currentMaskType.ToString());
            
            switch (_currentMaskType)
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