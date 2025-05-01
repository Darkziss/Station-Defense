using UnityEngine;
using Pooling;

namespace StationDefense
{
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        [SerializeField] private Transform _transform;

        [SerializeField] private Transform _rotatePointTransform;
        [SerializeField] private Transform _firePointTransform;

        [SerializeField] private Ball _ballPrefab;

        [SerializeField] private float _rotateSpeed;

        [SerializeField] private ColorTeam _team;

        public bool IsActive { get; private set; } = false;

        private const float aimLineAlphaEnabled = 1f;
        private const float aimLineAlphaDisabled = 0f;

        private const float fadeDuration = 0.3f;

        private void OnValidate()
        {
            if (_transform == null)
                _transform = transform;
        }

        private void Start()
        {
            DeathHandler.GameRestarted += () => _rotatePointTransform.rotation = Quaternion.identity;
        }

        private void Update()
        {
            if (!IsActive)
                return;
            
            Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            Vector2 mouseDirection = mouseWorldPosition - _rotatePointTransform.position;
            mouseDirection.Normalize();

            _rotatePointTransform.rotation = Quaternion.FromToRotation(Vector3.up, mouseDirection);
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Shoot()
        {
            Ball ball = PoolStorage.GetFromPool(nameof(Ball), _ballPrefab, _firePointTransform.position,
                Quaternion.identity);

            ball.Init(_team, _transform.up);
        }
    }
}