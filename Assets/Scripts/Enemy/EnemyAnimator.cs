using System;
using System.Collections;
using UnityEngine;
using PrimeTween;

namespace StationDefense
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Color32 _defaultColor;
        private readonly Color32 _actionColor = Color.white;

        private Coroutine _actionAnimationCoroutine;

        private readonly WaitForSeconds _actionAnimationDuration = new(ActionDuration);

        private readonly TweenSettings<float> _fadeOutSettings = new(0f, FadeOutDuration, ease: FadeEase);

        private bool IsNotOpaque => _spriteRenderer.color.a < 1f;

        private bool IsPlayingActionAnimation => _actionAnimationCoroutine != null;

        private const float ActionDuration = 0.15f;
        
        private const float FadeOutDuration = 0.5f;
        private const Ease FadeEase = Ease.Linear;

        private void OnValidate()
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void PlayActionAnimation()
        {
            if (IsPlayingActionAnimation)
                return;

            _actionAnimationCoroutine = StartCoroutine(ActionAnimation());
        }

        public void PlayDisableAnimation(Action callback)
        {
            Tween tween = Tween.Alpha(_spriteRenderer, _fadeOutSettings);

            if (callback != null)
                tween.OnComplete(callback);
        }

        public void ResetAll()
        {
            if (IsNotOpaque)
            {
                Color color = _spriteRenderer.color;
                color.a = 1f;

                _spriteRenderer.color = color;
            }
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