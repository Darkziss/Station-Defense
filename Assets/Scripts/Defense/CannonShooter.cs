using System;
using System.Collections;
using UnityEngine;
using Pooling;

namespace StationDefense
{
    public class CannonShooter : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _firePointTransform;

        [SerializeField] private Ball _ballPrefab;

        private Coroutine _shootCoroutine;

        private WaitForSeconds _shootDelay;

        public bool IsShooting => _shootCoroutine != null;

        private void OnValidate()
        {
            if (_transform == null)
                _transform = transform;

            if (_firePointTransform == null)
                _firePointTransform = GetComponentInChildren<Transform>();
        }

        public void Init(float shootDelay)
        {
            _shootDelay = new(shootDelay);
        }

        public void StartShooting(ColorTeam team)
        {
            if (IsShooting)
                throw new InvalidOperationException(nameof(IsShooting));

            _shootCoroutine = StartCoroutine(ShootWithDelay(team));
        }

        public void StopShooting()
        {
            if (!IsShooting)
                throw new InvalidOperationException(nameof(IsShooting));

            StopCoroutine(_shootCoroutine);
            _shootCoroutine = null;
        }

        private void InstantShoot(ColorTeam team)
        {
            Ball ball = PoolStorage.GetFromPool(_ballPrefab.BallName, _ballPrefab, 
                _firePointTransform.position, Quaternion.identity);

            ball.Init(team, _transform.up);
        }

        private IEnumerator ShootWithDelay(ColorTeam team)
        {
            while (true)
            {
                yield return _shootDelay;

                InstantShoot(team);
            }
        }
    }
}