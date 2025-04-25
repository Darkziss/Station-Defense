using UnityEngine;

namespace StationDefense
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        [SerializeField] private float rotationSpeed = 0.5f;

        private void OnValidate()
        {
            _transform = transform;
        }

        private void Update()
        {
            Vector3 localRotation = _transform.localRotation.eulerAngles;
            localRotation.z += rotationSpeed * Time.deltaTime;

            _transform.localRotation = Quaternion.Euler(localRotation);
        }
    }
}