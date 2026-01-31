using System.Collections;
using UnityEngine;

public class TemporaryPlatform : MonoBehaviour
{
    public float fallDelay = 1.5f;
    public float respawnDelay = 5.0f;

    private MeshRenderer meshRenderer;
    private Collider[] allColliders;
    private bool isCrumbling = false;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        // This gets both the Trigger and the Solid collider
        allColliders = GetComponents<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCrumbling)
        {
            Debug.Log("Player detected! Waiting " + fallDelay + " seconds before falling...");
            StartCoroutine(CrumbleRoutine());
        }
    }

    IEnumerator CrumbleRoutine()
    {
        isCrumbling = true;

        // --- PHASE 1: THE WARNING ---
        // The player is standing on the solid collider right now.
        yield return new WaitForSeconds(fallDelay);

        // --- PHASE 2: THE FALL ---
        Debug.Log("Platform disappearing now!");
        meshRenderer.enabled = false;
        
        // Disable ALL colliders (both the floor and the sensor)
        foreach (Collider c in allColliders)
        {
            c.enabled = false;
        }

        // --- PHASE 3: THE RECOVERY ---
        yield return new WaitForSeconds(respawnDelay);

        meshRenderer.enabled = true;
        foreach (Collider c in allColliders)
        {
            c.enabled = true;
        }
        
        isCrumbling = false;
    }
}