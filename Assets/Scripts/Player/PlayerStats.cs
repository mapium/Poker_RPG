using pokerpg;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Базовые статы персонажа")]
    public int baseHealth = 100;
    public int baseDamage = 10;
    public int baseDefense = 5;
    public int baseGold = 50; // Дадим начальное золото для теста

    [Header("Текущие статы")]
    public int currentHealth;
    public int currentDamage;
    public int currentDefense;
    public int currentGold;

    [Header("UI элементы для отображения статов")]
    public TMP_Text healthText;
    public TMP_Text damageText;
    public TMP_Text defenseText;
    public TMP_Text goldText;

    void Start()
    {
        currentHealth = baseHealth;
        currentDamage = baseDamage;
        currentDefense = baseDefense;
        currentGold = baseGold;

        UpdateUI();
    }

    public void ApplyCardStats(Card cardData)
    {
        currentHealth += cardData.health;
        currentDamage += cardData.damage;
        currentDefense += cardData.defense;
        currentGold += cardData.gold;

        UpdateUI();
    }

    // НОВЫЙ МЕТОД: Трата золота
    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            UpdateUI();
            Debug.Log($"Потрачено {amount} золота. Осталось: {currentGold}");
            return true;
        }
        else
        {
            Debug.Log($"Недостаточно золота! Нужно: {amount}, есть: {currentGold}");
            return false;
        }
    }

    // НОВЫЙ МЕТОД: Пополнение золота
    public void AddGold(int amount)
    {
        if (amount <= 0) return;
        currentGold += amount;
        UpdateUI();
        Debug.Log($"Получено {amount} золота. Всего: {currentGold}");
    }

    // Добавил публичный метод для обновления UI из других классов
    public void RefreshUI()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (healthText != null) healthText.text = $"HP: {currentHealth}";
        if (damageText != null) damageText.text = $"DMG: {currentDamage}";
        if (defenseText != null) defenseText.text = $"DEF: {currentDefense}";
        if (goldText != null) goldText.text = $"GOLD: {currentGold}";
    }
}