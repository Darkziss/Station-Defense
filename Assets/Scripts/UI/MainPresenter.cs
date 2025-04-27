using UnityEngine;
using TMPro;

namespace StationDefense.UI
{
    public class MainPresenter : MonoBehaviour
    {
        [SerializeField] private Base _base;
        
        [SerializeField] private TMP_Text _healthText;

        [SerializeField] private Panel _deathPanel;

        public void Init()
        {
            if (_deathPanel.IsVisible)
                _deathPanel.Hide();
            
            _base.HealthChanged += SetHealthText;

            DeathHandler.GameStopped += _deathPanel.Show;
            DeathHandler.GameRestarted += _deathPanel.Hide;
        }

        private void SetHealthText(int health)
        {
            _healthText.text = $"Health: {health}";
        }
    }
}