using UnityEngine;

namespace Utilities
{
    public class CircularProgressBar : MonoBehaviour
    {
        public float rotationSpeedDegrees = 270f;
        private Transform _transform;

        private void Start()
        {
            _transform = transform;
        }

        void Update()
        {
            _transform.Rotate(-Vector3.forward, Time.deltaTime * rotationSpeedDegrees);
        }
    }
}