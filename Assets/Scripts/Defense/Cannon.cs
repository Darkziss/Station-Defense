using UnityEngine;
using UnityEngine.InputSystem;
using StationDefense.InputSystem;

namespace StationDefense
{
    [RequireComponent(typeof(CannonShooter), typeof(CannonAnimator))]
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private Transform _rotatePointTransform;

        [SerializeField] private SpriteRenderer _baseSpriteRenderer;

        [SerializeField] private CannonShooter _baseShooter;
        [SerializeField] private CannonShooter _powerfulShooter;

        [SerializeField] private LineRenderer _laserSightLineRenderer;

        [SerializeField] private float _baseShootDelay = 0.1f;
        [SerializeField] private float _powerfulShootDelay = 1.5f;

        [SerializeField] private ColorTeam _team;

        private Color32 _activatedColor;
        private Color32 _deactivatedColor;

        private Camera _camera;

        private CannonAnimator _cannonAnimator;

        private InputAction _lookAction;

        private InputAction _shootAction;
        private InputAction _powerfulShootAction;

        public bool IsActive { get; private set; } = false;

        private const float DeactivatedColorLerp = 0.5f;

        private void OnValidate()
        {
            if (_camera == null)
                _camera = Camera.main;

            if (_cannonAnimator == null)
                _cannonAnimator = GetComponent<CannonAnimator>();
        }

        private void Start()
        {
            _activatedColor = TeamColorStorage.GetByTeam(_team);
            _deactivatedColor = Color32.Lerp(_activatedColor, Color.black, DeactivatedColorLerp);
            
            _baseSpriteRenderer.color = _deactivatedColor;

            _laserSightLineRenderer.enabled = false;
            
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

                StartPowerfulShooting();
            };

            DeathHandler.GameRestarted += () => _rotatePointTransform.rotation = Quaternion.identity;
        } 

        private void Update()
        {
            if (!IsActive)
                return;

            LookAtMouse();

            bool shouldStartShooting = _shootAction.WasPressedThisFrame() && !_baseShooter.IsShooting;
            bool shouldStopShooting = _shootAction.WasReleasedThisFrame() && _baseShooter.IsShooting;

            bool shouldStopPowerfulShooting = !_powerfulShootAction.IsPressed() 
                && _powerfulShooter.IsShooting;

            if (shouldStartShooting)
                _baseShooter.StartShooting(_team);
            else if (shouldStopShooting)
                _baseShooter.StopShooting();

            if (shouldStopPowerfulShooting)
                StopPowerfulShooting();
        }

        public void Activate()
        {
            IsActive = true;

            _baseSpriteRenderer.color = _activatedColor;

            _laserSightLineRenderer.enabled = true;

            if (_shootAction.IsPressed())
                _baseShooter.StartShooting(_team);

            if (_powerfulShootAction.IsPressed())
                StartPowerfulShooting();
        }

        public void Deactivate()
        {
            IsActive = false;

            _baseSpriteRenderer.color = _deactivatedColor;

            _laserSightLineRenderer.enabled = false;

            if (_baseShooter.IsShooting)
                _baseShooter.StopShooting();

            if (_powerfulShooter.IsShooting)
                StopPowerfulShooting();
        }

        private void LookAtMouse()
        {
            Vector2 mouseScreenPosition = _lookAction.ReadValue<Vector2>();
            Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(mouseScreenPosition);

            Vector2 mouseDirection = mouseWorldPosition - _rotatePointTransform.position;
            mouseDirection.Normalize();

            _rotatePointTransform.rotation = Quaternion.FromToRotation(Vector3.up, mouseDirection);
        }

        private void StartPowerfulShooting()
        {
            _powerfulShooter.StartShooting(_team);

            _cannonAnimator.StartPowerfulShootAnimation();
        }

        private void StopPowerfulShooting()
        {
            _powerfulShooter.StopShooting();

            _cannonAnimator.StopPowerfulShootAnimation();
        }
    }
}