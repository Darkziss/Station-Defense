using System;
using System.Collections;
using UnityEngine;
using PrimeTween;

namespace StationDefense
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Vector2 _defaultScale;

        private Color32 _defaultColor;
        private readonly Color32 _actionColor = Color.white;

        private Coroutine _actionAnimationCoroutine;
        private readonly WaitForSeconds _actionAnimationDuration = new(ActionDuration);

        private readonly TweenSettings<Vector3> _disableAnimationSettings = new(Vector3.zero,
            ScaleDuration, ease: ScaleEase);

        private readonly ShakeSettings _damageAnimationSettings = new(Vector3.one * ScaleShakeFactor,
            duration: ScaleShakeDuration, frequency: ScaleShakeFrequency);

        private bool IsPlayingActionAnimation => _actionAnimationCoroutine != null;

        private bool IsPlayingDamageAnimaton { get; set; } = false;

        private const float ActionDuration = 0.15f;
        
        private const float ScaleDuration = 0.3f;
        private const Ease ScaleEase = Ease.Linear;

        private const float ScaleShakeFactor = 1.05f;
        private const float ScaleShakeDuration = 0.3f;
        private const int ScaleShakeFrequency = 10;

        private void OnValidate()
        {
            if (_transform == null)
                _transform = transform;
            
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _defaultScale = _transform.localScale;
        }

        public void PlayActionAnimation()
        {
            if (IsPlayingActionAnimation)
                return;

            _actionAnimationCoroutine = StartCoroutine(ActionAnimation());
        }

        public void PlayDamageAnimation()
        {
            if (IsPlayingDamageAnimaton)
                return;

            IsPlayingDamageAnimaton = true;

            Tween.ShakeScale(_transform, _damageAnimationSettings)
                .OnComplete(() => IsPlayingDamageAnimaton = false);
        }

        public void PlayDisableAnimation(Action callback)
        {
            Tween tween = Tween.Scale(_transform, _disableAnimationSettings)
                .OnComplete(() =>
                {
                    _transform.localScale = _defaultScale;

                    callback?.Invoke();
                });
        }

        private IEnumerator ActionAnimation()
        {
            _defaultColor = _spriteRenderer.color;
            _spriteRenderer.color = _actionColor;

            yield return _actionAnimationDuration;

            _spriteRenderer.color = _defaultColor;

            _actionAnimationCoroutine = null;
        }
    }
}