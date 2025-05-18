using UnityEngine;
using Pooling;

namespace StationDefense
{
    [RequireComponent(typeof(Mover))]
    public class Ball : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TrailRenderer _trailRenderer;

        [SerializeField] private Mover _mover;

        [SerializeField] private string _ballName;

        [SerializeField] private int _baseDamage;

        [SerializeField] private bool _isDisposable = true;

        [SerializeField] private int _barrierLayer;
        [SerializeField] private int _enemyLayer;
        [SerializeField] private int _enemyBulletLayer;

        public string BallName => _ballName;

        public int BaseDamage => _baseDamage;

        public int ColorDamage => _baseDamage * damageMultiplier;

        public ColorTeam Team { get; private set; }

        private const int damageMultiplier = 3;

        private const byte TrailEndAlpha = 0;

        private void OnValidate()
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_trailRenderer == null)
                _trailRenderer = GetComponent<TrailRenderer>();

            if (_mover == null)
                _mover = GetComponent<Mover>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            int collisionLayer = collision.gameObject.layer;

            if (collisionLayer == _barrierLayer || collisionLayer == _enemyLayer 
                || (collisionLayer == _enemyBulletLayer && _isDisposable))
                Disable();
        }

        public void Init(ColorTeam team, Vector3 moveDirection)
        {
            Team = team;

            Color32 teamColor = TeamColorStorage.GetByTeam(team);

            _spriteRenderer.color = teamColor;

            SetTrailColor(teamColor);
            _trailRenderer.emitting = true;

            _mover.SetMoveDirection(moveDirection);
            _mover.StartMoving();
        }

        private void Disable()
        {
            PoolStorage.PutToPool(_ballName, this);

            _trailRenderer.emitting = false;
            _trailRenderer.Clear();
        }

        private void SetTrailColor(Color32 color)
        {
            Color32 endColor = new(color.r, color.g, color.b, TrailEndAlpha);

            _trailRenderer.startColor = color;
            _trailRenderer.endColor = endColor;
        }
    }
}