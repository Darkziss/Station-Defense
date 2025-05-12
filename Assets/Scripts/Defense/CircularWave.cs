using System;
using UnityEngine;
using PrimeTween;

namespace StationDefense
{
    public class CircularWave : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        
        [SerializeField] private int _damage;

        public bool IsExpanding { get; private set; } = false;

        public int Damage => _damage;

        private readonly Vector3 _startScale = Vector3.one * 0.1f;
        private readonly Vector3 _desiredScale = Vector3.one * 5f;

        private const float duration = 2f;

        private void OnValidate()
        {
            if (_transform == null)
                _transform = transform;
        }

        public void StartExpand()
        {
            if (IsExpanding)
                throw new InvalidOperationException(nameof(IsExpanding));

            IsExpanding = true;

            gameObject.SetActive(true);

            Tween.Scale(_transform, _startScale, _desiredScale, duration)
                .OnComplete(Disable);
        }

        private void Disable()
        {
            IsExpanding = false;

            gameObject.SetActive(false);
        }
    }
}