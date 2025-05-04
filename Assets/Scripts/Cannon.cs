using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using StationDefense.InputSystem;
using Pooling;
using PrimeTween;

namespace StationDefense
{
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        [SerializeField] private Transform _transform;

        [SerializeField] private Transform _rotatePointTransform;
        [SerializeField] private Transform _firePointTransform;

        [SerializeField] private Ball _ballPrefab;
        [SerializeField] private Ball _bigBallPrefab;

        [SerializeField] private ColorTeam _team;

        private Vector2 _defaultLocalPosition;

        private Coroutine _shootCoroutine;
        private Coroutine _powerfulShootCoroutine;

        private InputAction _lookAction;

        private InputAction _shootAction;
        private InputAction _powerfulShootAction;

        private readonly WaitForSeconds _shootDelay = new(0.1f);
        private readonly WaitForSeconds _powerfulShootDelay = new(1f);

        public bool IsActive { get; private set; } = false;

        private bool IsShooting => _shootCoroutine != null;
        private bool IsPowerfulShooting => _powerfulShootCoroutine != null;

        private void OnValidate()
        {
            if (_transform == null)
                _transform = transform;

            _defaultLocalPosition = _transform.localPosition;
        }

        private void Start()
        {
            _lookAction = InputHandler.LookAction;
            
            _shootAction = InputHandler.ShootAction;
            _powerfulShootAction = InputHandler.PowerfulShootAction;

            _lookAction.Enable();
            _shootAction.Enable();
            _powerfulShootAction.Enable();

            _powerfulShootAction.performed += (_) =>
            {
                if (!IsActive)
                    return;

                StartPowerfulShooting();

                Tween.ShakeLocalPosition(_transform, Vector3.one * 0.1f, 0.1f, frequency: 10, cycles: -1);
            };

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

            if (!_powerfulShootAction.IsPressed() && IsPowerfulShooting)
            {
                StopPowerfulShooting();

                Tween.StopAll(_transform);

                _transform.localPosition = _defaultLocalPosition;
            }
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

            if (IsPowerfulShooting)
                StopPowerfulShooting();

            Tween.StopAll(_transform);

            _transform.localPosition = _defaultLocalPosition;
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

        private void StartPowerfulShooting()
        {
            _powerfulShootCoroutine = StartCoroutine(PowerfulShootWithDelay());
        }

        private void StopPowerfulShooting()
        {
            StopCoroutine(_powerfulShootCoroutine);
            _powerfulShootCoroutine = null;
        }

        private void InstantShoot(Ball ballPrefab)
        {
            Ball ball = PoolStorage.GetFromPool(ballPrefab.BallName, ballPrefab, _firePointTransform.position,
                Quaternion.identity);

            ball.Init(_team, _transform.up);
        }

        private void InstantBasicShoot() => InstantShoot(_ballPrefab);

        private void InstantPowerfulShoot() => InstantShoot(_bigBallPrefab);

        private IEnumerator ShootWithDelay()
        {
            while (true)
            {
                yield return _shootDelay;

                InstantBasicShoot();
            }
        }

        private IEnumerator PowerfulShootWithDelay()
        {
            while (true)
            {
                yield return _powerfulShootDelay;

                InstantPowerfulShoot();
            }
        }
    }
}