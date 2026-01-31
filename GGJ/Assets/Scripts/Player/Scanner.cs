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
        private Throw throwScript;

        private void Start()
        {
            throwScript = GetComponent<Throw>();
        }

        private void FixedUpdate()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * offset, collisionRadius, layerMask);

            _result = null;
            
            for (int i = 0; i < hits.Length; i++)
            {
                Interactable interactable = hits[i].GetComponent<Interactable>();
                
                if (interactable != null && throwScript.Item != interactable.GetComponent<Rigidbody>())
                {
                    Debug.Log(interactable.name);
                    _result = interactable;
                    break;
                }
            }
        }

        public void InteractInputPressed()
        {
            if (_result == null) return;
            
            _result?.Interact();
            
            if (_result is BasicPickup)
            {
                Rigidbody rb = _result.GetComponent<Rigidbody>();

                Debug.Log("Basic Pickup: " + rb.gameObject.name);

                //If holding a BasicPickup, release it
                throwScript.ReleaseRigidbodyAndForget();
                
                //If currently holding a mask, release it, inform the MaskManager
                if (throwScript.Item && throwScript.Item.GetComponent<MaskPickup>())
                {
                    var currentMask = throwScript.Item?.GetComponent<MaskPickup>();
                    currentMask.Throw();
                    GetComponent<MaskManager>().MaskDropped(currentMask.GetMaskType());
                }
                
                //Unset rigidbody settings of previously held items
                if (throwScript.Item)
                {
                    throwScript.Item.isKinematic = false;
                    throwScript.Item.detectCollisions = true;
                    throwScript.Item.transform.SetParent(null);
                }
                
                //If new pickup, hold it
                throwScript.AttachRigidbody(rb, _result is MaskPickup);
                ((BasicPickup)_result).currentParent = throwScript;

                //If new pickup is mask, inform the MaskManager
                if (_result is MaskPickup)
                {
                    Debug.Log("Mask Pickup: " + _result.gameObject.name);
                    GetComponent<MaskManager>().MaskPickedUp(((MaskPickup)_result).GetMaskType());
                }
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