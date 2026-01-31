using UnityEngine;

namespace Utilities
{
    public class PickupHalo : MonoBehaviour
    {
        [Header("Pulse Settings")]
        [SerializeField] float pulseSpeed = 2f;
        [SerializeField] float rotationSpeed = 270f;
        [SerializeField] Vector2 pulseScaleRange = new Vector2(0.01f, 0.09f);
    
        [Header("Billboard Settings")]
        [SerializeField] bool useStaticBillboard = true; // True for top-down, False for VR/Free-look

        private Vector3 baseScale;
        private Camera mainCam;
        private float currentRotation = 0f;

        void Start()
        {
            baseScale = transform.localScale;
            mainCam = Camera.main;
        }

        void LateUpdate()
        {
            HandlePulse();
            HandleBillboard();
        }

        void HandlePulse()
        {
            float scaleOffset = Mathf.Lerp(pulseScaleRange.x, pulseScaleRange.y, (1f + Mathf.Sin(Time.time * pulseSpeed)) * 0.5f);
            transform.localScale = Vector3.one * scaleOffset; //baseScale + Vector3.one * scaleOffset;
        }

        void HandleBillboard()
        {
            currentRotation += rotationSpeed * Time.deltaTime;
            
            if (useStaticBillboard)
            {
                // Simple approach: Match camera rotation
                // Perfect for fixed top-down cameras
                transform.rotation = mainCam.transform.rotation;
            }
            else
            {
                // Dynamic approach: Look directly at camera position
                // Better if the camera orbits or rotates significantly
                transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.forward,
                    mainCam.transform.rotation * Vector3.up);
            }
            
            //Spin along axis
            transform.Rotate(-Vector3.forward, currentRotation, Space.Self);
        }
    }
}
