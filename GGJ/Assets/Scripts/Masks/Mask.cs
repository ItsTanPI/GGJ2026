using UnityEngine;

namespace Masks
{
    public delegate void MaskEvent();
    
    public abstract class Mask : MonoBehaviour
    {
        public event MaskEvent OnMaskActivated;
        public event MaskEvent OnMaskDeactivated;

        private bool _isMaskActive = false;
        
        public virtual void Activate()
        {
            _isMaskActive = true;
            OnMaskActivated?.Invoke();
        }

        public virtual void Deactivate()
        {
            _isMaskActive = false;
            OnMaskDeactivated?.Invoke();
        }
        
        public bool IsMaskActive() => _isMaskActive;
    }
}