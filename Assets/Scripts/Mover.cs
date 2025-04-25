using UnityEngine;

namespace StationDefense
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        [SerializeField] private bool _isMoving = false;

        [SerializeField] private float _moveSpeed = 3f;
        [SerializeField] private Vector3 _moveDirection;

        public bool IsMoving => _isMoving;

        private void OnValidate()
        {
            _transform = transform;

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

        public void SetMoveDirection(Vector3 direction)
        {
            _moveDirection = direction;
        }
    }
}