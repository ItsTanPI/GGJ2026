using UnityEngine;

namespace Masks
{
    public enum MaskType
    {
        None,
        DashMask,
        ShrinkMask
    }
    
    public delegate void GeneralEvent();
    
    public abstract class Mask : MonoBehaviour
    {
        public event GeneralEvent OnMaskActivated;
        public event GeneralEvent OnMaskDeactivated;

        private bool _isActive = false;
        private MaskType _type = MaskType.None;
        
        public bool IsActive => _isActive;
        public MaskType Type => _type;
        
        public virtual void Activate()
        {
            _isActive = true;
            OnMaskActivated?.Invoke();
        }

        public virtual void Deactivate()
        {
            _isActive = false;
            OnMaskDeactivated?.Invoke();
        }
    }
}