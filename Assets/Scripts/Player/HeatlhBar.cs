using UnityEngine;
using UnityEngine.UI;

public class HeatlhBar : MonoBehaviour
{
    [SerializeField] private Image _HealthBar;
    [SerializeField] private PlayerStats _playerStats;

    private void Update()
    {
        if (_HealthBar != null && _playerStats != null && _playerStats.baseHealth > 0)
        {
            _HealthBar.fillAmount = (float)_playerStats.currentHealth / _playerStats.baseHealth;
        }
    }
}
