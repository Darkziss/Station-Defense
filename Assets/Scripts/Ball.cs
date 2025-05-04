using UnityEngine;
using Pooling;

namespace StationDefense
{
    [RequireComponent(typeof(Mover))]
    public class Ball : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Mover _mover;

        [SerializeField] private string _ballName;

        [SerializeField] private int _baseDamage;

        [SerializeField] private int _barrierLayer;
        [SerializeField] private int _enemyLayer;

        public string BallName => _ballName;

        public int BaseDamage => _baseDamage;

        public int ColorDamage => _baseDamage * damageMultiplier;

        public ColorTeam Team { get; private set; }

        private const int damageMultiplier = 3;

        private void OnValidate()
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_mover == null)
                _mover = GetComponent<Mover>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            int collisionLayer = collision.gameObject.layer;

            if (collisionLayer == _barrierLayer || collisionLayer == _enemyLayer)
                Disable();
        }

        public void Init(ColorTeam team, Vector3 moveDirection)
        {
            Team = team;

            _spriteRenderer.color = TeamColorStorage.GetByTeam(team);

            _mover.SetMoveDirection(moveDirection);
            _mover.StartMoving();
        }

        private void Disable() => PoolStorage.PutToPool(_ballName, this);
    }
}