using UnityEngine;

namespace StationDefense
{
    public class TargetMover : MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        [SerializeField] private bool _isMoving = false;

        [SerializeField, Min(0f)] private float moveSpeed = 1f;

        private void OnValidate()
        {
            if (_transform == null)
                _transform = transform;
        }

        private void Update()
        {
            if (!_isMoving)
                return;

            _transform.position = Vector2.MoveTowards(_transform.position, Vector3.zero,
                moveSpeed * Time.deltaTime);
        }

        public void StartMoving()
        {
            if (_isMoving)
                return;

            _isMoving = true;
        }

        public void StopMoving()
        {
            if (!_isMoving)
                return;

            _isMoving = false;
        }
    }
}