using System;
using UnityEngine;
using Pooling;
using PrimeTween;

namespace StationDefense
{
    [RequireComponent(typeof(BoxCollider2D), typeof(BaseEnemyMover), typeof(Health))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private BoxCollider2D _boxCollider;

        [SerializeField] private BaseEnemyMover _baseEnemyMover;

        [SerializeField] private Health _health;

        [SerializeField] private string _enemyName;

        [SerializeField] private int _ballLayer;
        [SerializeField] private int _baseLayer;

        public string EnemyName => _enemyName;

        public ColorTeam Team { get; private set; }

        private const float fadeDuration = 0.5f;

        public static event Action<bool> EnemyHit;

        private void OnValidate()
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_boxCollider == null)
                _boxCollider = GetComponent<BoxCollider2D>();

            if (_baseEnemyMover)
                _baseEnemyMover = GetComponent<BaseEnemyMover>();

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

            _baseEnemyMover.StartMoving();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == _ballLayer)
            {
                Ball ball = collision.gameObject.GetComponent<Ball>();

                bool isSameTeam = ball.Team == Team;

                int desiredDamage = isSameTeam ? ball.ColorDamage : ball.BaseDamage;

                _health.ChangeHealth(-desiredDamage);

                if (_health.IsHealthAtZero)
                    Disable(true);

                EnemyHit?.Invoke(isSameTeam);
            }

            if (collision.gameObject.layer == _baseLayer)
                Disable(true);
        }

        private void Disable(bool playAnimation)
        {
            void PutToPool() => PoolStorage.PutToPool(_enemyName, this);

            _boxCollider.enabled = false;

            _baseEnemyMover.StopMoving();
            _baseEnemyMover.StopAttack();

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