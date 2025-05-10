using System.Collections;
using UnityEngine;
using Pooling;

namespace StationDefense
{
    public class BaseEnemyMover : MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        [SerializeField] private EnemyBullet _bulletPrefab;

        [SerializeField] private bool _isMoving = false;

        [SerializeField, Min(0f)] private float _moveSpeed = 1f;
        [SerializeField, Min(0f)] private float _attackMoveSpeed = 0.5f;

        [SerializeField] private float _attackDistance = 5f;

        private ColorTeam _team;

        private bool _isAttacking = false;

        private Coroutine _changeDirectionCoroutine;
        private Coroutine _shootCoroutine;

        private Vector2 _moveDirection;

        private WaitForSeconds _shootDelay;

        private readonly Vector3 _targetPosition = Vector3.zero;

        private readonly WaitForSeconds _changeDirectionDelay = new(0.5f);

        private void OnValidate()
        {
            if (_transform == null)
                _transform = transform;
        }

        public void Init(ColorTeam team, float shootDelay)
        {
            _team = team;

            _shootDelay = new(shootDelay);
        }

        private void Update()
        {
            if (!_isMoving)
                return;
            
            if (_isAttacking)
            {
                Vector2 translate = _attackMoveSpeed * Time.deltaTime * _moveDirection;

                _transform.Translate(translate);
            }
            else
            {
                float distance = Vector2.Distance(_transform.position, _targetPosition);

                if (distance > _attackDistance)
                {
                    _transform.position = Vector2.MoveTowards(_transform.position, _targetPosition,
                        _moveSpeed * Time.deltaTime);
                }
                else
                {
                    StartAttack();
                }
            }
        }

        public void StartMoving()
        {
            if (_isMoving)
                return;

            _isMoving = true;
        }

        public void StopMoving()
        {
            if (!_isMoving)
                return;

            _isMoving = false;
        }

        public void StopAttack()
        {
            if (!_isAttacking)
                return;

            _isAttacking = false;

            StopCoroutine(_changeDirectionCoroutine);
            StopCoroutine(_shootCoroutine);
        }

        private void StartAttack()
        {
            _isAttacking = true;

            _changeDirectionCoroutine = StartCoroutine(ChangeDirectionWithDelay());
            _shootCoroutine = StartCoroutine(ShootWithDelay());
        }

        private IEnumerator ChangeDirectionWithDelay()
        {
            while (true)
            {
                const int min = -1;
                const int max = 2;

                int x = Random.Range(min, max);
                int y = Random.Range(min, max);

                _moveDirection = new Vector2(x, y);

                yield return _changeDirectionDelay;
            }
        }

        private IEnumerator ShootWithDelay()
        {
            while (true)
            {
                yield return _shootDelay;

                Vector2 shootDirection = _targetPosition - _transform.position;
                shootDirection.Normalize();

                Quaternion rotation = Quaternion.FromToRotation(Vector2.up, shootDirection);

                EnemyBullet bullet = PoolStorage.GetFromPool(_bulletPrefab.BulletName, _bulletPrefab,
                    _transform.position, rotation);

                bullet.Init(_team, shootDirection);
            }
        }
    }
}