using System;
using UnityEngine;

namespace StationDefense
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int _currentHealth = 0;
        [SerializeField] private int _maxHealth = 10;

        public bool IsHealthAtZero => _currentHealth == minHealth;

        private const int minHealth = 0;

        public event Action<int> HealthChanged;

        private void Start()
        {
            ChangeHealth(_maxHealth);
        }

        public void ChangeHealth(int amount)
        {
            _currentHealth += amount;

            _currentHealth = Mathf.Clamp(_currentHealth, minHealth, _maxHealth);

            HealthChanged?.Invoke(_currentHealth);
        }

        public void RestoreHealth() => ChangeHealth(_maxHealth);
    }
}