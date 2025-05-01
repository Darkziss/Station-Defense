using UnityEngine;
using Pooling;
using System.Collections;

namespace StationDefense
{
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        [SerializeField] private Transform _transform;

        [SerializeField] private Transform _rotatePointTransform;
        [SerializeField] private Transform _firePointTransform;

        [SerializeField] private Ball _ballPrefab;

        [SerializeField] private bool _canShoot;
        [SerializeField] private ColorTeam _team;

        private Coroutine _shootCoroutine;

        private readonly WaitForSeconds _shootDelay = new(0.1f);

        public bool IsActive { get; private set; } = false;

        private bool IsShooting => _shootCoroutine != null;

        private const int shootMouseButton = 0;

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

            LookAtMouse();

            bool shootInput = Input.GetMouseButton(shootMouseButton);

            if (shootInput && !IsShooting)
                StartShooting();
            else if (!shootInput && IsShooting)
                StopShooting();
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;

            if (IsShooting)
                StopShooting();
        }

        private void LookAtMouse()
        {
            Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            Vector2 mouseDirection = mouseWorldPosition - _rotatePointTransform.position;
            mouseDirection.Normalize();

            _rotatePointTransform.rotation = Quaternion.FromToRotation(Vector3.up, mouseDirection);
        }

        private void StartShooting()
        {
            _shootCoroutine = StartCoroutine(ShootWithDelay());
        }

        private void StopShooting()
        {
            StopCoroutine(_shootCoroutine);
            _shootCoroutine = null;
        }

        private void InstantShoot()
        {
            Ball ball = PoolStorage.GetFromPool(nameof(Ball), _ballPrefab, _firePointTransform.position,
                Quaternion.identity);

            ball.Init(_team, _transform.up);
        }

        private IEnumerator ShootWithDelay()
        {
            while (true)
            {
                yield return _shootDelay;

                InstantShoot();
            }
        }

    }
}