using UnityEngine;
using TMPro;
using Pooling;
using PrimeTween;

namespace StationDefense
{
    public class BubbleText : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TMP_Text _text;

        private readonly WaitForSeconds _disableDelay = new(FadeOutDelay);

        private readonly TweenSettings _positionYAnimationSettings = new(PositionYDuration, 
            ease: PositionYEase);

        private readonly TweenSettings<float> _fadeOutSettings = new(0f, FadeOutDuration,
            ease: FadeOutEase, startDelay: FadeOutDelay);

        private float EndYPosition => _rectTransform.position.y + OffsetY;

        private const float OffsetY = 0.3f;

        private const float PositionYDuration = 0.3f;
        private const Ease PositionYEase = Ease.Linear;

        private const float FadeOutDuration = 0.5f;
        private const Ease FadeOutEase = Ease.Linear;
        private const float FadeOutDelay = 0.8f;

        private void OnValidate()
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();

            if (_text == null)
                _text = GetComponent<TMP_Text>();
        }

        public void SetupAndDisableAfterDelay(string text, Color32 color, Material material)
        {
            _text.text = text;
            _text.color = color;
            _text.fontMaterial = material;

            Sequence.Create()
                .Chain(Tween.PositionY(_rectTransform, EndYPosition, _positionYAnimationSettings))
                .Chain(Tween.Alpha(_text, _fadeOutSettings))
                .OnComplete(Disable);
        }

        private void Disable()
        {
            PoolStorage.PutToPool(nameof(BubbleText), this);
        }
    }
}