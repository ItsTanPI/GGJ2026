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
        
        private MaskType _currentMaskType = MaskType.None;
        public MaskType CurrentMaskType => _currentMaskType;

        //Cached input
        private Vector2 _moveInput = Vector2.zero;
        private Vector2 _lookInput = Vector2.zero;

        public Vector2 MoveInput => _moveInput;
        public Vector2 LookInput => _lookInput;
        
        public void MaskPickedUp(MaskType maskType)
        {
            Debug.Log("Mask Picked Up: " + maskType);
            OnMaskChanged?.Invoke(_currentMaskType, maskType);
            _currentMaskType = maskType;
            if (maskType == MaskType.KeyMask)
                GetComponent<Key>().TurnON();
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
                case MaskType.KeyMask:
                    GetComponent<Key>().TurnOff();
                    break;
                case MaskType.NecroMask:
                    GetComponent<Necro>().KillSkeleton();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _currentMaskType = MaskType.None;
        }
        
        public void TryActivateCurrentMask()
        {
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
                case MaskType.KeyMask:
                    GetComponent<Key>().TurnON();
                    break;
                case MaskType.NecroMask:
                    GetComponent<Necro>().FireNecroProjectile();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        //For necromask
        public void CacheInput(Vector2 moveInput, Vector2 lookInput)
        {
            _moveInput = moveInput;
            _lookInput = lookInput;
        }
    }
}