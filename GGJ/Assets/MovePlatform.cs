using System.Collections;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform pointA;
    public Transform pointB;
    public float speed = 2.0f;
    
    private Vector3 target;
    private bool isMoving = true;

    void Start()
    {
        // Set the initial target
        target = pointB.position;
    }

    void Update()
    {
        if (!isMoving) return;
        
        // Constant movement between points
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Switch targets when the point is reached
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            target = (target == pointA.position) ? pointB.position : pointA.position;
        }
    }

    public void StopRoutine()
    {
        isMoving = false;
    }

    public void StartRoutine()
    {
        isMoving = true;
    }
}