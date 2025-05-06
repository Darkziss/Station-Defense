using UnityEngine;
using Pooling;

namespace StationDefense
{
    [RequireComponent(typeof(BoxCollider2D), typeof(Mover))]
    public class EnemyBullet : MonoBehaviour
    {
        [SerializeField] private Mover _mover;

        [SerializeField] private string _bulletName;

        public string BulletName => _bulletName;

        private void OnValidate()
        {
            if (_mover == null)
                _mover = GetComponent<Mover>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Disable();
        }

        public void Init(Vector2 moveDirection)
        {
            _mover.SetMoveDirection(moveDirection);
            _mover.StartMoving();
        }

        private void Disable() => PoolStorage.PutToPool(_bulletName, this);
    }
}