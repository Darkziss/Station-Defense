using System;
using UnityEngine;

namespace StationDefense
{
    public class ScoreTracker : MonoBehaviour
    {
        [SerializeField] private WaveTracker _waveTracker;
        [SerializeField] private Base _base;
        
        [SerializeField] private int _score;

        [SerializeField] private int _currentQuota;

        private const int scoreIncrement = 10;
        private const int scoreDecrement = -50;

        private const int defaultQuota = 100;
        private const int quotaIncrement = 100;

        public event Action<int, int> ScoreChanged;

        public void Init()
        {
            BaseEnemy.EnemyHit += ChangeScore;

            _currentQuota = defaultQuota;

            _waveTracker.NewWaveStarted += (_) => CheckAndIncrementQuota();

            ScoreChanged?.Invoke(_score, _currentQuota);
        }

        private void ChangeScore(bool isSameTeam)
        {
            if (isSameTeam)
                _score += scoreIncrement;
            else
                _score += scoreDecrement;

            ScoreChanged?.Invoke(_score, _currentQuota);
        }

        private void CheckAndIncrementQuota()
        {
            if (_score < _currentQuota)
                _base.Damage();

            _currentQuota += quotaIncrement;

            ScoreChanged?.Invoke(_score, _currentQuota);
        }
    }
}