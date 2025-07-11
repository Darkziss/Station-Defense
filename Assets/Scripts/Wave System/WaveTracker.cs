using System;
using System.Collections;
using UnityEngine;

namespace StationDefense
{
    public class WaveTracker : MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;

        [SerializeField] private int _currentWave = 0;
        [SerializeField] private float _currentWaveDuration = defaultWaveDuration;

        private Coroutine _waveCycleCoroutine;

        private readonly MutableWaitForSeconds _waveDuration = new();
        private readonly WaitForSeconds _waveCooldown = new(10f);

        private const float defaultWaveDuration = 10f;

        private const float waveDurationIncrement = 3f;

        public event Action<int> FirstWaveStarted;
        public event Action<int> NewWaveStarted;
        public event Action<int> WaveEnded;

        public void StartWaveCycle()
        {
            _currentWave++;

            _waveDuration.SetSeconds(_currentWaveDuration);

            _waveCycleCoroutine = StartCoroutine(WaveCycle());

            FirstWaveStarted?.Invoke(_currentWave);
        }

        public void StopWaveCycle()
        {
            _enemySpawner.StopSpawn();

            StopCoroutine(_waveCycleCoroutine);

            _currentWave = 0;
            _currentWaveDuration = defaultWaveDuration;
        }

        private IEnumerator WaveCycle()
        {
            while (true)
            {
                _enemySpawner.StartSpawn();

                yield return _waveDuration;

                _enemySpawner.StopSpawn();

                WaveEnded?.Invoke(_currentWave);

                yield return _waveCooldown;

                _currentWave++;
                _currentWaveDuration += waveDurationIncrement;
                _waveDuration.SetSeconds(_currentWaveDuration);

                NewWaveStarted?.Invoke(_currentWave);
            }
        }
    }
}