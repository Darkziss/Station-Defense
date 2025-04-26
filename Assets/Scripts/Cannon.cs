using UnityEngine;
using Pooling;
using PrimeTween;

namespace StationDefense
{
    [RequireComponent(typeof(Rotator))]
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private Rotator _rotator;

        [SerializeField] private Transform _transform;

        [SerializeField] private Transform _firePointTransform;
        [SerializeField] private SpriteRenderer _aimLineSpriteRenderer;

        [SerializeField] private Mover _ballPrefab;

        private const float aimLineAlphaEnabled = 1f;
        private const float aimLineAlphaDisabled = 0f;

        private const float fadeDuration = 0.3f;

        private void OnValidate()
        {
            if (_rotator == null)
                _rotator = GetComponent<Rotator>();

            if (_transform == null)
                _transform = transform;
        }

        public void Activate()
        {
            _rotator.StartRotating();

            _aimLineSpriteRenderer.gameObject.SetActive(true);
            Tween.Alpha(_aimLineSpriteRenderer, aimLineAlphaEnabled, fadeDuration);
        }

        public void Deactivate()
        {
            _rotator.StopRotating();

            Tween.Alpha(_aimLineSpriteRenderer, aimLineAlphaDisabled, fadeDuration)
                .OnComplete(() => _aimLineSpriteRenderer.gameObject.SetActive(false));
        }

        public void Shoot()
        {
            Deactivate();

            //Mover ballMover = Instantiate(_ballPrefab, _firePointTransform.position, Quaternion.identity);
            Mover ballMover = PoolStorage.GetFromPool(nameof(Ball), _ballPrefab, _firePointTransform.position,
                Quaternion.identity);

            ballMover.SetMoveDirection(_transform.up);
            ballMover.StartMoving();
        }
    }
}