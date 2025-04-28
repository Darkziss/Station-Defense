using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace StationDefense.UI
{
    public class MainPresenter : MonoBehaviour
    {
        [SerializeField] private Base _base;
        [SerializeField] private WaveTracker _waveTracker;
        
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private TMP_Text _waveText;

        [SerializeField] private TMP_Text _tipText;

        [SerializeField] private Panel _deathPanel;

        [SerializeField] private Button _startWaveCycleButton;

        public void Init()
        {
            if (_deathPanel.IsVisible)
                _deathPanel.Hide();
            
            _base.HealthChanged += SetHealthText;

            _waveTracker.NewWaveStarted += SetWaveText;
            _waveTracker.WaveEnded += SetWaveEndedText;

            DeathHandler.GameStopped += _deathPanel.Show;
            DeathHandler.GameRestarted += _deathPanel.Hide;
            DeathHandler.GameRestarted += () =>
            {
                _tipText.gameObject.SetActive(true);
                _startWaveCycleButton.gameObject.SetActive(true);
            };
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