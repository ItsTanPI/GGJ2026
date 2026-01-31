using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrink : MonoBehaviour
{
    [SerializeField] private float baseScale = 1f;
    [SerializeField] private float shrunkScale = 0.3f;
    [SerializeField] private float shrunkDuration = 0.2f;
    [SerializeField] private float shrinkCooldown = 2f;
    [SerializeField] private bool isShrunk = false;
    
    private bool isShrinking = false;
    private bool isCoolingDown = false;
    
    public void TryToggleShrink()
    {
        if(isShrinking) return;
        if(isCoolingDown) return;
        
        StopAllCoroutines();
        StartCoroutine(nameof(ToggleShrinkCoroutine));
    }

    IEnumerator ToggleShrinkCoroutine()
    {
        float timer = 0f;
        const float c4 = 2 * Mathf.PI / 3;

        isShrinking = true;
        
        while (timer < shrunkDuration)
        {
            float normalizedTime = timer / shrunkDuration;

            float t = Mathf.Pow(2, -10 * normalizedTime) * Mathf.Sin((normalizedTime * 10f - 0.75f) * c4);
            
            float scale = isShrunk ? Mathf.Lerp(baseScale, shrunkScale, t) : Mathf.Lerp(shrunkScale, baseScale, t);
            transform.localScale = Vector3.one * scale;
            
            timer += Time.deltaTime;
            yield return null;
        }
        
        isShrinking = false;
        isShrunk = !isShrunk;
        
        StartCoroutine(nameof(Cooldown));
    }
    
    IEnumerator Cooldown()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(shrinkCooldown);
        isCoolingDown = false;
    }

    public void RevertShrink()
    {
        if (isShrunk || isShrinking)
        {
            StopAllCoroutines();
            StartCoroutine(nameof(ToggleShrinkCoroutine));
        }
    }
}
