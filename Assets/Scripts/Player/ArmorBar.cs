using UnityEngine;
using UnityEngine.UI;

public class ArmorBar : MonoBehaviour
{
    [SerializeField] private Image _ArmorBar;
    [SerializeField] private PlayerStats _playerStats;

    private void Update()
    {
        if (_ArmorBar != null && _playerStats != null && _playerStats.baseDefense > 0)
        {
            _ArmorBar.fillAmount = (float)_playerStats.currentDefense / _playerStats.baseDefense;
        }
    }
}
