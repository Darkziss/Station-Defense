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

        private Vector2 _originalScale;
        private Color32 _originalColor;

        private Coroutine _actionAnimationCoroutine;
        private readonly WaitForSeconds _actionAnimationDuration = new(ActionDuration);

        private readonly TweenSettings<float> _fadeInSettings = new(1f, 0f, FadeInDuration,
            ease: FadeInEase, cycles: FadeInCycles, FadeInCycleMode);

        private readonly TweenSettings<Vector3> _disableAnimationSettings = new(Vector3.zero,
            ScaleDuration, ease: ScaleEase);

        private readonly ShakeSettings _damageAnimationSettings = new(Vector3.one * ScaleShakeFactor,
            duration: ScaleShakeDuration, frequency: ScaleShakeFrequency);

        private Color32 ActionColor => Color.Lerp(_originalColor, Color.white, ActionColorFactor);

        private bool IsPlayingSpawnAnimation { get; set; } = false;
        private bool IsPlayingActionAnimation => _actionAnimationCoroutine != null;

        private const float FadeInDuration = 0.1f;
        private const Ease FadeInEase = Ease.Linear;
        private const int FadeInCycles = 4;
        private const CycleMode FadeInCycleMode = CycleMode.Yoyo;

        private const float ActionDuration = 0.15f;
        private const float ActionColorFactor = 0.8f;
        
        private const float ScaleDuration = 0.3f;
        private const Ease ScaleEase = Ease.Linear;

        private const float ScaleShakeFactor = 0.5f;
        private const float ScaleShakeDuration = 0.3f;
        private const int ScaleShakeFrequency = 5;

        private void OnValidate()
        {
            if (_transform == null)
                _transform = transform;
            
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _originalScale = _transform.localScale;
        }

        public void PlaySpawnAnimation()
        {
            IsPlayingSpawnAnimation = true;
            
            Tween.Alpha(_spriteRenderer, _fadeInSettings)
                .OnComplete(() => IsPlayingSpawnAnimation = false);
        }

        public void PlayActionAnimation()
        {
            if (IsPlayingSpawnAnimation || IsPlayingActionAnimation)
                return;

            _actionAnimationCoroutine = StartCoroutine(ActionAnimation());
        }

        public void PlayDamageAnimation()
        {
            Tween.ShakeScale(_transform, _damageAnimationSettings);
        }

        public void PlayDisableAnimation(Action callback)
        {
            Tween tween = Tween.Scale(_transform, _disableAnimationSettings)
                .OnComplete(() =>
                {
                    _transform.localScale = _originalScale;

                    callback?.Invoke();
                });
        }

        private IEnumerator ActionAnimation()
        {
            _originalColor = _spriteRenderer.color;
            _spriteRenderer.color = ActionColor;

            yield return _actionAnimationDuration;

            _spriteRenderer.color = _originalColor;

            _actionAnimationCoroutine = null;
        }
    }
}