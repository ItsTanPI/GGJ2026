using System;
using Masks;
using UnityEngine;

namespace Player
{
    public delegate void MaskEvent(MaskType oldType, MaskType newType);
    
    public class MaskManager : MonoBehaviour
    {
        public event MaskEvent OnMaskChanged;
        
        private Mask _currentMask = null;

        public void MaskPickedUp(MaskType maskType)
        {
            OnMaskChanged?.Invoke(_currentMask == null ? MaskType.None : _currentMask.Type, maskType);

            if (_currentMask != null) Destroy(_currentMask);

            switch (maskType)
            {
                case MaskType.None:
                    break;
                case MaskType.MeleeMask:
                    _currentMask = gameObject.AddComponent<MeleeMask>();
                    break;
                case MaskType.RangedMask:
                    break;
                case MaskType.KeyMask:
                    break;
                case MaskType.PlatformMask:
                    break;
                case MaskType.ForceFieldMask:
                    break;
                default:
                    break;
            }
        }
    }
}