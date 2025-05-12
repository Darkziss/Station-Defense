using UnityEngine;
using Pooling;

namespace StationDefense
{
    [RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(Mover))]
    public class EnemyBullet : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private Mover _mover;

        [SerializeField] private string _bulletName;

        public string BulletName => _bulletName;

        public ColorTeam Team { get; private set; }

        private void OnValidate()
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_mover == null)
                _mover = GetComponent<Mover>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Disable();
        }

        public void Init(ColorTeam team, Vector2 moveDirection)
        {
            Team = team;

            _spriteRenderer.color = TeamColorStorage.GetByTeam(team);

            _mover.SetMoveDirection(moveDirection);
            _mover.StartMoving();
        }

        public void Disable()
        {
            if (_mover.IsMoving)
                _mover.StopMoving();

            PoolStorage.PutToPool(_bulletName, this);
        }
    }
}