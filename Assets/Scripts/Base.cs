using System;
using UnityEngine;
using UnityEngine.InputSystem;
using StationDefense.InputSystem;

namespace StationDefense
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(BaseShooter))]
    public class Base : MonoBehaviour
    {
        [SerializeField] private CannonSelector _cannonSelector;
        
        [SerializeField] private BaseShooter _baseShooter;

        [SerializeField] private int _health = 0;
        [SerializeField] private int _maxHealth = 10;

        private InputAction _circleShootAction;

        public int Health => _health;

        public bool IsDead => _health == 0;

        private const int damage = 1;

        public event Action<int> HealthChanged;
        public event Action BaseDied;

        private void OnValidate()
        {
            if (_baseShooter == null)
                _baseShooter = GetComponent<BaseShooter>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!IsDead)
                Damage();
        }

        public void Init()
        {
            _health = _maxHealth;

            _circleShootAction = InputHandler.CircleShootAction;

            _circleShootAction.Enable();

            _circleShootAction.performed += (_) => CircleShoot();

            HealthChanged?.Invoke(_health);
        }

        public void Damage()
        {
            if (IsDead)
                throw new InvalidOperationException(nameof(IsDead));

            _health -= damage;

            HealthChanged?.Invoke(_health);
            
            if (IsDead)
                BaseDied?.Invoke();
        }

        private void CircleShoot()
        {
            if (!_cannonSelector.HaveSelectedCannon || IsDead)
                return;

            ColorTeam team = _cannonSelector.SelectedTeam;

            _baseShooter.Shoot(team);
        }
    }
}