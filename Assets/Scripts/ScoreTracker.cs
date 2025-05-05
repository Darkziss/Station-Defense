using System;
using UnityEngine;

namespace StationDefense
{
    public class ScoreTracker : MonoBehaviour
    {
        [SerializeField] private int _score;

        private const int scoreIncrement = 10;
        private const int scoreDecrement = -50;

        public event Action<int> ScoreChanged;

        public void Init()
        {
            Enemy.EnemyHit += ChangeScore;
        }

        private void ChangeScore(bool isSameTeam)
        {
            if (isSameTeam)
                _score += scoreIncrement;
            else
                _score += scoreDecrement;

            ScoreChanged?.Invoke(_score);
        }
    }
}