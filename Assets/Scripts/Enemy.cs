using UnityEngine;
using Pooling;
using PrimeTween;

namespace StationDefense
{
    [RequireComponent(typeof(BoxCollider2D), typeof(TargetMover), typeof(Health))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private BoxCollider2D _boxCollider;
        
        [SerializeField] private TargetMover _mover;

        [SerializeField] private Health _health;

        [SerializeField] private int _ballLayer;
        [SerializeField] private int _baseLayer;

        public ColorTeam Team { get; private set; }

        private const float fadeDuration = 0.5f;

        private void OnValidate()
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_boxCollider == null)
                _boxCollider = GetComponent<BoxCollider2D>();

            if (_mover == null)
                _mover = GetComponent<TargetMover>();

            if (_health == null)
                _health = GetComponent<Health>();
        }

        private void Start()
        {
            DeathHandler.GameRestarted += () => Disable(false);
        }

        private void OnEnable()
        {
            if (_spriteRenderer.color.a < 1f)
            {
                Color color = _spriteRenderer.color;
                color.a = 1f;

                _spriteRenderer.color = color;
            }

            _boxCollider.enabled = true;

            _health.RestoreHealth();
        }

        public void Init(ColorTeam team)
        {
            Team = team;

            _spriteRenderer.color = TeamColorStorage.GetByTeam(team);

            _mover.StartMoving();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == _ballLayer)
            {
                Ball ball = collision.gameObject.GetComponent<Ball>();

                int desiredDamage = ball.Team == Team ? ball.ColorDamage : ball.BaseDamage;

                _health.ChangeHealth(-desiredDamage);

                if (_health.IsHealthAtZero)
                    Disable(true);
            }

            if (collision.gameObject.layer == _baseLayer)
                Disable(true);
        }

        private void Disable(bool playAnimation)
        {
            void PutToPool() => PoolStorage.PutToPool(nameof(Enemy), this);

            _boxCollider.enabled = false;

            _mover.StopMoving();

            if (playAnimation)
            {
                Tween.Alpha(_spriteRenderer, 0f, fadeDuration)
                    .OnComplete(PutToPool);
            }
            else
            {
                PutToPool();
            }
        }
    }
}