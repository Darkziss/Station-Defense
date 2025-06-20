using System;
using UnityEngine;
using PrimeTween;

namespace StationDefense
{
    [RequireComponent(typeof(CircleRenderer), typeof(CircleCollider2D))]
    public class CircularWave : MonoBehaviour
    {
        [SerializeField] private int _damage;

        private Transform _transform;

        private CircleRenderer _circleRenderer;
        private CircleCollider2D _collider;

        private readonly TweenSettings<Vector3> _scaleTweenSettings = new(Vector3.one * StartScaleFactor, 
            Vector3.one * DesiredScaleFactor, scaleDuration, scaleEase);
        private readonly TweenSettings<float> _fadeTweenSettings = new(1f, 0f, fadeDuration, fadeEase);

        public bool IsExpanding { get; private set; } = false;

        public int Damage => _damage;

        private const float StartScaleFactor = 0.1f;
        private const float DesiredScaleFactor = 6f;

        private const float scaleDuration = 1.5f;
        private const Ease scaleEase = Ease.OutQuart;

        private const float fadeDuration = 0.3f;
        private const Ease fadeEase = Ease.Linear;

        private void OnValidate()
        {
            if (_transform == null)
                _transform = transform;

            if (_circleRenderer == null)
                _circleRenderer = GetComponent<CircleRenderer>();

            if (_collider == null)
                _collider = GetComponent<CircleCollider2D>();
        }

        public void StartExpand()
        {
            if (IsExpanding)
                throw new InvalidOperationException(nameof(IsExpanding));

            IsExpanding = true;

            _circleRenderer.SetAlpha(1f);
            _collider.enabled = true;

            gameObject.SetActive(true);

            Sequence.Create()
                .Chain(Tween.Scale(_transform, _scaleTweenSettings))
                .ChainCallback(() => _collider.enabled = false)
                .Chain(Tween.Custom(_fadeTweenSettings, _circleRenderer.SetAlpha))
                .ChainCallback(Disable);
        }

        private void Disable()
        {
            IsExpanding = false;

            gameObject.SetActive(false);
        }
    }
}