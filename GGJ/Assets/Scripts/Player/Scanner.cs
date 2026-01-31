using System;
using Interactables;
using Interaction;
using UnityEngine;

namespace Player
{
    public class Scanner : MonoBehaviour
    {
        [SerializeField] private float collisionRadius = 1f;
        [SerializeField] private float offset = 1f;
        [SerializeField] private LayerMask layerMask;

        private Interactable _result = null;
        
        private void FixedUpdate()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * offset, collisionRadius, layerMask);

            _result = null;
            
            for (int i = 0; i < hits.Length; i++)
            {
                Interactable interactable = hits[i].GetComponent<Interactable>();
                
                if (interactable != null)
                {
                    //Debug.Log(interactable.name);
                    _result = interactable;
                    break;
                }
            }
        }

        public void InteractInputPressed()
        {
            if (_result == null) return;
            
            _result?.Interact();
            
            if (_result?.GetInteractionType() == InteractionType.Mask)
            {
                MaskPickup mask = (MaskPickup)_result;
                if (mask.currentParent == null)
                {
                    GetComponent<MaskManager>().MaskPickedUp(mask.GetMaskType());
                }
            }

            if (_result?.GetInteractionType() == InteractionType.Pickup)
            {
                //Do pickup stuff
            }
            
            _result?.Consume();
            _result = null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.forward * offset, collisionRadius);
        }
    }
}