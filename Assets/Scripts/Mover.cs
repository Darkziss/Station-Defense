using UnityEngine;

namespace StationDefense
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        [SerializeField] private bool _isMoving = false;

        [SerializeField, Min(0f)] private float _moveSpeed = 1f;
        [SerializeField] private Vector3 _moveDirection;

        public Vector3 MoveDirection
        {
            get => _moveDirection;
            set => _moveDirection = value.normalized;
        }

        public bool IsMoving => _isMoving;

        private void OnValidate()
        {
            if (_transform == null) _transform = transform;

            _moveDirection = _moveDirection.normalized;
        }

        private void Update()
        {
            if (!_isMoving)
                return;

            Vector3 translate = _moveSpeed * Time.deltaTime * _moveDirection;

            _transform.Translate(translate);
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