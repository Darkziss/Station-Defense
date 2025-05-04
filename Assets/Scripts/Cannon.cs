using UnityEngine;
using UnityEngine.InputSystem;
using StationDefense.InputSystem;
using PrimeTween;

namespace StationDefense
{
    [RequireComponent(typeof(CannonShooter))]
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        [SerializeField] private Transform _transform;

        [SerializeField] private CannonShooter _baseShooter;
        [SerializeField] private CannonShooter _powerfulShooter;

        [SerializeField] private Transform _rotatePointTransform;

        [SerializeField] private float _baseShootDelay = 0.1f;
        [SerializeField] private float _powerfulShootDelay = 1.5f;

        [SerializeField] private ColorTeam _team;

        private Vector2 _defaultLocalPosition;

        private InputAction _lookAction;

        private InputAction _shootAction;
        private InputAction _powerfulShootAction;

        private readonly ShakeSettings _cannonShakeSettings = new(Vector3.one * shakeFactor, shakeDuration,
            frequency: shakeFrequency, cycles: shakeCycles);

        public bool IsActive { get; private set; } = false;

        private const float shakeFactor = 0.1f;
        private const float shakeDuration = 0.1f;
        private const float shakeFrequency = 10f;
        private const int shakeCycles = -1;

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

            _baseShooter.Init(_baseShootDelay);
            _powerfulShooter.Init(_powerfulShootDelay);

            _powerfulShootAction.performed += (_) =>
            {
                if (!IsActive)
                    return;

                _powerfulShooter.StartShooting(_team);

                Tween.ShakeLocalPosition(_transform, _cannonShakeSettings);
            };

            DeathHandler.GameRestarted += () => _rotatePointTransform.rotation = Quaternion.identity;
        } 

        private void Update()
        {
            if (!IsActive)
                return;

            LookAtMouse();

            if (_shootAction.WasPressedThisFrame() && !_baseShooter.IsShooting)
                _baseShooter.StartShooting(_team);
            else if (_shootAction.WasReleasedThisFrame() && _baseShooter.IsShooting)
                _baseShooter.StopShooting();

            if (!_powerfulShootAction.IsPressed() && _powerfulShooter.IsShooting)
            {
                _powerfulShooter.StopShooting();

                Tween.StopAll(_transform);

                _transform.localPosition = _defaultLocalPosition;
            }
        }

        public void Activate()
        {
            IsActive = true;

            if (_shootAction.IsPressed())
                _baseShooter.StartShooting(_team);

            if (_powerfulShootAction.IsPressed())
                _powerfulShooter.StartShooting(_team);
        }

        public void Deactivate()
        {
            IsActive = false;

            if (_baseShooter.IsShooting)
                _baseShooter.StopShooting();

            if (_powerfulShooter.IsShooting)
                _powerfulShooter.StopShooting();

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
    }
}