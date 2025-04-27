using UnityEngine;
using Pooling;
using PrimeTween;

namespace StationDefense
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(TargetMover))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private BoxCollider2D _boxCollider;
        
        [SerializeField] private TargetMover _mover;

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
        }

        private void OnEnable()
        {
            Color color = _spriteRenderer.color;
            color.a = 1f;

            _spriteRenderer.color = color;

            _boxCollider.enabled = true;
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

                if (ball.Team == Team)
                {
                    Disable();
                    return;
                }
            }

            if (collision.gameObject.layer == _baseLayer)
                Disable();
        }

        private void Disable()
        {
            _boxCollider.enabled = false;

            _mover.StopMoving();

            Tween.Alpha(_spriteRenderer, 0f, fadeDuration)
                .OnComplete(() => PoolStorage.PutToPool(nameof(Enemy), this));
        }
    }
}