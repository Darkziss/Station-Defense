using System;
using UnityEngine;

namespace StationDefense
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class Base : MonoBehaviour
    {
        [SerializeField] private int _health = 0;
        [SerializeField] private int _maxHealth = 10;

        public int Health => _health;

        public bool IsDead => _health == 0;

        private const int damage = 1;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!IsDead)
                Damage();
        }

        public void Init()
        {
            _health = _maxHealth;
        }

        public void Damage()
        {
            if (IsDead)
                throw new InvalidOperationException(nameof(IsDead));

            _health -= damage;
        }
    }
}