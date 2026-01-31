using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onPlatePressed;
    public UnityEvent onPlateReleased;

    [Header("Visual Settings")]
    public float pressedHeight = 0.3f; 
    public float idleHeight = 0.6f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Down");
            // Moves THIS object's transform
            transform.localPosition = new Vector3(transform.localPosition.x, pressedHeight, transform.localPosition.z);
            onPlatePressed.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Up");
            // Moves THIS object's transform
            transform.localPosition = new Vector3(transform.localPosition.x, idleHeight, transform.localPosition.z);
            onPlateReleased.Invoke();
        }
    }
}