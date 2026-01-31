using System;
using UnityEngine;

namespace Utilities
{
    public class NecroProjectile : MonoBehaviour
    {
        public System.Action<Vector3> OnHit;
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Necro projectile hitL: " + other.gameObject.name);
            OnHit?.Invoke(transform.position);
        }
    }
}