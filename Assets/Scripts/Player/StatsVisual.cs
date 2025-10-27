using TMPro;
using UnityEngine;
public class StatsVisual : MonoBehaviour
{
    [Header("Источник данных")]
    [Tooltip("Если не задано — попытается найти Player.Instance -> PlayerStats или FindObjectOfType<PlayerStats>()")]
    [SerializeField] private PlayerStats playerStats;

    [Header("UI элементы для отображения")]
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text damageText;

    // Кэшированные значения, чтобы обновлять текст только при изменении
    private int _lastGold = int.MinValue;
    private int _lastDamage = int.MinValue;

    private void Awake()
    {
        if (playerStats == null)
        {
            if (Player.Instance != null)
                playerStats = Player.Instance.GetComponent<PlayerStats>();

            if (playerStats == null)
                playerStats = FindObjectOfType<PlayerStats>();
        }
    }

    private void Start()
    {
        Refresh();
    }

    private void Update()
    {
        if (playerStats == null) return;

        // Обновляем только при изменении значений
        if (playerStats.currentGold != _lastGold || playerStats.currentDamage != _lastDamage)
            Refresh();
    }

    // Публичный метод для принудительного обновления (можно вызвать из других классов)
    public void Refresh()
    {
        if (playerStats == null) return;

        if (goldText != null)
            goldText.text = playerStats.currentGold.ToString();

        if (damageText != null)
            damageText.text = playerStats.currentDamage.ToString();

        _lastGold = playerStats.currentGold;
        _lastDamage = playerStats.currentDamage;
    }
}