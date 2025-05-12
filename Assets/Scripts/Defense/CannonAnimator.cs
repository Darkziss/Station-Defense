using System;
using UnityEngine;
using PrimeTween;

namespace StationDefense
{
    public class CannonAnimator : MonoBehaviour
    {
        private Transform _transform;

        private Vector2 _defaultLocalPosition;

        private readonly ShakeSettings _cannonShakeSettings = new(Vector3.one * shakeFactor, shakeDuration,
            frequency: shakeFrequency, cycles: shakeCycles);

        public bool IsPowerfulShootAnimationPlaying { get; private set; } = false;

        private const float shakeFactor = 0.1f;
        private const float shakeDuration = 0.1f;
        private const float shakeFrequency = 10f;
        private const int shakeCycles = -1;

        private void OnValidate()
        {
            if (_transform == null)
                _transform = transform;
        }

        private void Start()
        {
            _defaultLocalPosition = _transform.localPosition;
        }

        public void StartPowerfulShootAnimation()
        {
            if (IsPowerfulShootAnimationPlaying)
                throw new InvalidOperationException(nameof(IsPowerfulShootAnimationPlaying));

            IsPowerfulShootAnimationPlaying = true;

            Tween.ShakeLocalPosition(_transform, _cannonShakeSettings);
        }

        public void StopPowerfulShootAnimation()
        {
            if (!IsPowerfulShootAnimationPlaying)
                return;

            IsPowerfulShootAnimationPlaying = false;

            Tween.StopAll(_transform);

            _transform.localPosition = _defaultLocalPosition;
        }
    }
}