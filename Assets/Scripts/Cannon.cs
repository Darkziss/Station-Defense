using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using StationDefense.InputSystem;
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

        [SerializeField] private bool _canShoot;
        [SerializeField] private ColorTeam _team;

        private Coroutine _shootCoroutine;

        private InputAction _lookAction;
        private InputAction _shootAction;

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
            _lookAction = InputHandler.LookAction;
            _shootAction = InputHandler.ShootAction;

            _lookAction.Enable();
            _shootAction.Enable();

            DeathHandler.GameRestarted += () => _rotatePointTransform.rotation = Quaternion.identity;
        } 

        private void Update()
        {
            if (!IsActive)
                return;

            LookAtMouse();

            if (_shootAction.WasPressedThisFrame() && !IsShooting)
                StartShooting();
            else if (_shootAction.WasReleasedThisFrame() && IsShooting)
                StopShooting();
        }

        public void Activate()
        {
            IsActive = true;

            if (_shootAction.IsPressed())
                StartShooting();
        }

        public void Deactivate()
        {
            IsActive = false;

            if (IsShooting)
                StopShooting();
        }

        private void LookAtMouse()
        {
            Vector2 mouseScreenPosition = _lookAction.ReadValue<Vector2>();
            Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(mouseScreenPosition);

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