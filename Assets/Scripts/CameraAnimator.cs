using UnityEngine;
using PrimeTween;

namespace StationDefense
{
    [RequireComponent(typeof(Camera))]
    public class CameraAnimator : MonoBehaviour
    {
        [SerializeField] private Transform _operatorTransform;

        [SerializeField] private Base _base;

        private Transform _transform;
        private Camera _camera;

        private bool _isPositionAnimationPlaying = false;

        private static readonly TweenSettings _positionAnimationSettings = new(PositionDuration,
            ease: PositionEase);

        private const float PositionDuration = 0.5f;
        private const Ease PositionEase = Ease.OutCubic;

        private const float ShakeStrengthFactor = 0.8f;
        private const float ShakeDuration = 0.3f;
        private const int ShakeFrequency = 10;

        private void OnValidate()
        {
            if (_transform == null)
                _transform = transform;

            if (_camera == null)
                _camera = GetComponent<Camera>();
        }

        private void Start()
        {
            _base.HealthChanged += (_) => PlayShakeAnimation();
        }

        public void MoveToPositionWithoutAnimation(Vector3 position)
        {
            _operatorTransform.position = position;
        }

        public void MoveToPositionWithAnimation(Vector3 position)
        {
            if (_isPositionAnimationPlaying)
                Tween.StopAll(_operatorTransform);
            else
                _isPositionAnimationPlaying = true;

            Tween.Position(_operatorTransform, position, _positionAnimationSettings)
                .OnComplete(() => _isPositionAnimationPlaying = false);
        }

        public void PlayShakeAnimation()
        {
            Tween.ShakeCamera(_camera, ShakeStrengthFactor, duration: ShakeDuration,
                frequency: ShakeFrequency);
        }
    }
}