using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Portal : MonoBehaviour
{
    public LayerMask portalAllowedLayers;
    public float portalRadius = 2f;
    public bool Disable = false;
    private Vector2 lookInput;
    private bool currentPointValid = false;
    private Vector2 currentPoint, portalStart, portalEnd;
    private bool portalStartSet = false, portalEndSet = false;

    public void LookInput(Vector2 lookInput)
    {
        this.lookInput = Mouse.current.position.ReadValue();
        
        Ray camRay = Camera.main.ScreenPointToRay(this.lookInput);
        RaycastHit hit;
        if (Physics.Raycast(camRay, out hit, Mathf.Infinity, portalAllowedLayers))
        {
            currentPoint = hit.point;
            currentPointValid = (Physics.OverlapSphere(hit.point, portalRadius).Length == 0 &&
                                 Vector3.Distance(hit.point, portalEnd) > 3 * portalRadius);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = currentPointValid ? Color.white  : Color.yellow;
        Gizmos.DrawWireSphere(currentPoint, portalRadius);

        if(portalStartSet)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(portalStart, portalRadius);
        }

        if(portalEndSet)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(portalEnd, portalRadius);
        }
    }
}
