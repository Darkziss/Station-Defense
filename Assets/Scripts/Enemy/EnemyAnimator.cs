using System;
using UnityEngine;
using PrimeTween;

namespace StationDefense
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private readonly TweenSettings<float> _fadeOutSettings = new(0f, duration);

        private const float duration = 0.5f;

        private void OnValidate()
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void PlayDisableAnimation(Action callback)
        {
            Tween tween = Tween.Alpha(_spriteRenderer, _fadeOutSettings);

            if (callback != null)
                tween.OnComplete(callback);
        }

        public void ResetAll()
        {
            if (_spriteRenderer.color.a < 1f)
            {
                Color color = _spriteRenderer.color;
                color.a = 1f;

                _spriteRenderer.color = color;
            }
        }
    }
}