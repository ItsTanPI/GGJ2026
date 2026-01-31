using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] private float dashDistance = 3f;
    [SerializeField] private float dashDuration = 0.2f;
    private bool isDashing = false;
    private Vector3 dashDirection = Vector3.zero;
    private Vector3 startPosition;

    public void TryDash(Vector3 direction)
    {
        if (isDashing) return;
        dashDirection = direction;
        startPosition = transform.position;
        
        StopCoroutine(nameof(DashCoroutine));
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + dashDirection * dashDistance, 0.5f);
    }
}
