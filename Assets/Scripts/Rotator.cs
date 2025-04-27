using UnityEngine;

namespace StationDefense
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Transform _targetTransform;

        [SerializeField] private bool _isRotating = false;

        [SerializeField] private float _rotationAngle = 0f;

        public bool IsRotating => _isRotating;

        private void Update()
        {
            if (!_isRotating)
                return;
            
            Vector3 localRotation = _targetTransform.localRotation.eulerAngles;
            localRotation.z += _rotationAngle * Time.deltaTime;

            _targetTransform.localRotation = Quaternion.Euler(localRotation);
        }

        public void InverseAngle() => _rotationAngle = -_rotationAngle;

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