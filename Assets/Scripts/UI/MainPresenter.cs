using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace StationDefense.UI
{
    public class MainPresenter : MonoBehaviour
    {
        [SerializeField] private Base _base;
        [SerializeField] private WaveTracker _waveTracker;
        [SerializeField] private ScoreTracker _scoreTracker;
        
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private TMP_Text _waveText;
        [SerializeField] private TMP_Text _scoreText;

        [SerializeField] private Panel _deathPanel;

        [SerializeField] private Button _startWaveCycleButton;

        public void Init()
        {
            if (_deathPanel.IsVisible)
                _deathPanel.Hide();
            
            _base.HealthChanged += SetHealthText;

            _waveTracker.NewWaveStarted += SetWaveText;
            _waveTracker.WaveEnded += SetWaveEndedText;

            _scoreTracker.ScoreChanged += SetScoreText;

            DeathHandler.GameStopped += _deathPanel.Show;
            DeathHandler.GameRestarted += _deathPanel.Hide;
            DeathHandler.GameRestarted += () =>
            {
                _startWaveCycleButton.gameObject.SetActive(true);
            };
        }

        private void SetScoreText(int score)
        {
            _scoreText.text = $"Score: {score}";
        }

        private void SetHealthText(int health)
        {
            _healthText.text = $"Health: {health}";
        }

        private void SetWaveText(int wave)
        {
            _waveText.text = $"Wave: {wave}";
        }

        private void SetWaveEndedText(int wave)
        {
            _waveText.text = $"Wave {wave} is over! (The next wave starts in 10 seconds)";
        }
    }
}