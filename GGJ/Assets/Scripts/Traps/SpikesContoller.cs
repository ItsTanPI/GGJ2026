using System.Collections;
using UnityEngine;

public class SpikesController : MonoBehaviour
{
    [Header("References")]
    public Transform spikeMesh;
    private Vector3 originalPosition;

    [Header("Timing Settings")]
    public float timeUp = 5.0f;    // Longer duration
    public float timeDown = 1.0f;  // Shorter duration
    public float moveSpeed = 2.0f;

    [Header("Positioning")]
    public Vector3 retractOffset = new Vector3(0, 0, -0.02f);
    
    private Vector3 extendedPosition;
    private Vector3 retractedPosition;

    void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        health?.Death(); 
    }

    void Start()
    {
        originalPosition = spikeMesh.localPosition;
        extendedPosition = spikeMesh.localPosition;
        retractedPosition = extendedPosition - retractOffset;
        StartCoroutine(SpikeRoutine());
    }

    IEnumerator SpikeRoutine()
    {
        spikeMesh.localPosition = retractedPosition;

        while (true)
        {
            // Move Up
            yield return StartCoroutine(LerpPosition(retractedPosition));
            yield return new WaitForSeconds(timeUp);

            // Move Down
            yield return StartCoroutine(LerpPosition(extendedPosition));
            yield return new WaitForSeconds(timeDown);
        }
    }

    IEnumerator LerpPosition(Vector3 target)
    {
        float timeElapsed = 0;
        float duration = 1.0f / moveSpeed; // Higher moveSpeed makes duration shorter
        Vector3 startPosition = spikeMesh.localPosition;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;

            // Easing Equation: This creates a smooth "Ease In Out" curve
            // t = t * t * (3f - 2f * t); 
            
            // Alternatively, for a simple "Ease Out" (Fast start, slow finish):
            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            spikeMesh.localPosition = Vector3.Lerp(startPosition, target, t);
            yield return null;
        }

        spikeMesh.localPosition = target;
    }

    public void StopRoutine()
    {
        StopAllCoroutines();
        StartCoroutine(LerpPosition(originalPosition));
    }

    public void StartRoutine()
    {
        StartCoroutine(SpikeRoutine());
    }
}