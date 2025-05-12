using System;
using UnityEngine;
using PrimeTween;

namespace StationDefense
{
    [RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D))]
    public class CircularWave : MonoBehaviour
    {
        [SerializeField] private int _damage;

        [SerializeField] private Color32 _defaultColor = Color.black;

        private Transform _transform;

        private SpriteRenderer _spriteRenderer;
        private CircleCollider2D _collider;

        private readonly Vector3 _startScale = Vector3.one * 0.1f;
        private readonly Vector3 _desiredScale = Vector3.one * 5f;

        private readonly TweenSettings<float> _fadeTweenSettings = new(0f, fadeDuration, fadeEase);

        public bool IsExpanding { get; private set; } = false;

        public int Damage => _damage;

        private const float scaleDuration = 1.5f;
        private const Ease scaleEase = Ease.OutQuart;

        private const float fadeDuration = 0.3f;
        private const Ease fadeEase = Ease.Linear;

        private void OnValidate()
        {
            if (_transform == null)
                _transform = transform;

            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_collider == null)
                _collider = GetComponent<CircleCollider2D>();
        }

        public void StartExpand()
        {
            if (IsExpanding)
                throw new InvalidOperationException(nameof(IsExpanding));

            IsExpanding = true;

            _spriteRenderer.color = _defaultColor;
            _collider.enabled = true;

            gameObject.SetActive(true);

            Sequence.Create()
                .Chain(Tween.Scale(_transform, _startScale, _desiredScale, scaleDuration, scaleEase))
                .ChainCallback(() => _collider.enabled = false)
                .Chain(Tween.Alpha(_spriteRenderer, _fadeTweenSettings))
                .ChainCallback(Disable);
        }

        private void Disable()
        {
            IsExpanding = false;

            gameObject.SetActive(false);
        }
    }
}