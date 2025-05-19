using UnityEngine;
using PrimeTween;

namespace StationDefense
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Shield : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private ColorTeam _defenseTeam = ColorTeam.Red;

        private static readonly TweenSettings _colorAnimationSettings = new(ColorAnimationDuration,
            ease: ColorAnimationEase);

        private bool _isPlayingColorAnimation = false;

        private const float ColorAnimationDuration = 0.15f;
        private const Ease ColorAnimationEase = Ease.Linear;

        private void OnValidate()
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _spriteRenderer.color = TeamColorStorage.GetByTeam(_defenseTeam);
        }

        public void ChangeDefenseTeam(ColorTeam team)
        {
            _defenseTeam = team;

            if (_isPlayingColorAnimation)
                Tween.StopAll(_spriteRenderer);

            _isPlayingColorAnimation = true;

            Color targetColor = TeamColorStorage.GetByTeam(team);

            Tween.Color(_spriteRenderer, targetColor, _colorAnimationSettings)
                .OnComplete(() => _isPlayingColorAnimation = false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            EnemyBullet bullet = collision.gameObject.GetComponent<EnemyBullet>();

            if (bullet.Team == _defenseTeam)
                bullet.Disable();
        }
    }
}