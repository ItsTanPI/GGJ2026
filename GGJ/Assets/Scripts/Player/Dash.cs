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

    public void TryDash()
    {
        if (isDashing) return;
        if(isCoolingDown) return;
        dashDirection = transform.forward;
        startPosition = transform.position;
        
        StopAllCoroutines();
        StartCoroutine(nameof(DashCoroutine));
    }

    IEnumerator DashCoroutine()
    {
        isDashing = true;
        GetComponent<Movement>().enabled = false;

        float radius = 0.5f;
        float finalDistance = dashDistance;

        Vector3 targetPosition = startPosition + dashDirection * finalDistance;
        
        float timer = 0f;

        while (timer < dashDuration)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / dashDuration;
            
            float easedTime = 1f - Mathf.Pow(1f - normalizedTime, 3); 
            transform.position = Vector3.Lerp(startPosition, targetPosition, easedTime);
            
            yield return null;
        }
        
        transform.position = targetPosition;
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
