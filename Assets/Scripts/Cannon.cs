using UnityEngine;
using Pooling;

namespace StationDefense
{
    [RequireComponent(typeof(Rotator))]
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private Rotator _rotator;

        [SerializeField] private Transform _firePointTransform;

        [SerializeField] private Ball _ballPrefab;

        [SerializeField] private ColorTeam _team;

        private const float aimLineAlphaEnabled = 1f;
        private const float aimLineAlphaDisabled = 0f;

        private const float fadeDuration = 0.3f;

        private void OnValidate()
        {
            if (_rotator == null)
                _rotator = GetComponent<Rotator>();

            if (_transform == null)
                _transform = transform;
        }

        private void Start()
        {
            DeathHandler.GameRestarted += _rotator.ResetRotation;
        }

        public void Activate()
        {
            _rotator.StartRotating();
        }

        public void Deactivate()
        {
            _rotator.StopRotating();
        }

        public void InverseAngle() => _rotator.InverseAngle();

        public void Shoot()
        {
            Ball ball = PoolStorage.GetFromPool(nameof(Ball), _ballPrefab, _firePointTransform.position,
                Quaternion.identity);

            ball.Init(_team, _transform.up);
        }
    }
}