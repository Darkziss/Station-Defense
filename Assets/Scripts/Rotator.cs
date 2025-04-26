using UnityEngine;

namespace StationDefense
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Transform _targetTransform;

        [SerializeField] private bool _isRotating = false;

        [SerializeField] private float rotationSpeed = 1f;

        public bool IsRotating => _isRotating;

        private void Update()
        {
            if (!_isRotating)
                return;
            
            Vector3 localRotation = _targetTransform.localRotation.eulerAngles;
            localRotation.z += rotationSpeed * Time.deltaTime;

            _targetTransform.localRotation = Quaternion.Euler(localRotation);
        }

        public void StartRotating()
        {
            if (_isRotating)
                return;

            _isRotating = true;
        }

        public void StopRotating()
        {
            if (!_isRotating)
                return;

            _isRotating = false;
        }
    }
}