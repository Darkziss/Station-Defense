using System;
using UnityEngine;

namespace StationDefense
{
    public class Wallet : MonoBehaviour
    {
        [SerializeField] private int _balance = 0;

        public int Balance => _balance;

        private const int minBalance = 0;

        public void Set(int newBalance)
        {
            if (newBalance < minBalance)
                throw new ArgumentException(nameof(newBalance));

            _balance = newBalance;
        }

        public void Add(int amount)
        {
            _balance += amount;
        }

        public bool TrySubtract(int amount)
        {
            if (amount > _balance)
                return false;

            _balance -= amount;

            return true;
        }
    }
}