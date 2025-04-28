using System;
using UnityEngine;

namespace StationDefense
{
    public class DeathHandler : MonoBehaviour
    {
        [SerializeField] private Base _base;

        [SerializeField] private WaveTracker _waveTracker;
        [SerializeField] private CannonSelector _cannonSelector;

        public bool IsGameStopped { get; private set; } = false;

        public static event Action GameStopped;
        public static event Action GameRestarted;

        public void Init()
        {
            _base.BaseDied += StopGame;
        }

        public void RestartGame()
        {
            if (!IsGameStopped)
                throw new InvalidOperationException(nameof(IsGameStopped));

            IsGameStopped = false;

            _cannonSelector.ResetAll();
            _cannonSelector.gameObject.SetActive(true);

            Time.timeScale = 1;

            GameRestarted?.Invoke();
        }

        private void StopGame()
        {
            if (IsGameStopped)
                throw new InvalidOperationException(nameof(IsGameStopped));

            IsGameStopped = true;
            
            Time.timeScale = 0;

            _waveTracker.StopWaveCycle();

            _cannonSelector.gameObject.SetActive(false);

            GameStopped?.Invoke();
        }
    }
}