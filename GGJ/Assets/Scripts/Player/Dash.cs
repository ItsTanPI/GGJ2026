using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] private float dashDistance = 3f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 2f;
    
    private bool isCoolingDown = false;
    private bool isDashing = false;
    private Vector3 dashDirection = Vector3.zero;
    private Vector3 startPosition;
    public LayerMask collisionMask;

    public void TryDash(Vector3 direction)
    {
        if (isDashing) return;
        if(isCoolingDown) return;
        dashDirection = direction;
        startPosition = transform.position;
        
        StopAllCoroutines();
        StartCoroutine(nameof(DashCoroutine));
    }

    IEnumerator DashCoroutine()
    {
        isDashing = true;
        GetComponent<Movement>().enabled = false;

        float radius = 0.5f;              // Player thickness
        float skin = 0.1f;               // Small offset so we don’t clip into walls
        float finalDistance = dashDistance;

        RaycastHit hit;

        // Check if something blocks the dash
        if (Physics.SphereCast(startPosition, radius, dashDirection, out hit, dashDistance, collisionMask))
        {
            finalDistance = hit.distance - skin; // Stop before wall
        }

        Vector3 targetPosition = startPosition + dashDirection * finalDistance;

        float timer = 0f;

        while (timer < dashDuration)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / dashDuration;

            float easedTime = 1f - Mathf.Pow(1f - normalizedTime, 3);
            Vector3 nextPos = Vector3.Lerp(startPosition, targetPosition, easedTime);

            // Extra safety in case something moves into us mid-dash
            if (Physics.CheckSphere(nextPos, radius, collisionMask))
                break;

            transform.position = nextPos;

            yield return null;
        }

        GetComponent<Movement>().enabled = true;
        isDashing = false;

        StartCoroutine(Cooldown());
    }


    IEnumerator Cooldown()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(dashCooldown);
        isCoolingDown = false;
    }

    //Called when mask dropped
    public void RevertDash()
    {
        StopAllCoroutines();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + dashDirection * dashDistance, 0.5f);
    }
}
