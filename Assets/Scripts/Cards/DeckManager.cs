using UnityEngine;
using System.Collections.Generic;
using pokerpg;

public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();
    private List<Card> availableCards = new List<Card>();
    private int cardsDrawnCount = 0; // Счетчик взятых карт

    void Start()
    {
        // Загрузить все ассеты карт из папки ресурсов
        Card[] cards = Resources.LoadAll<Card>("Cards");
        allCards.AddRange(cards);
        availableCards.AddRange(allCards);

        // Перемешать колоду
        ShuffleDeck();

        // Раздать начальные карты
        HandManager hand = FindObjectOfType<HandManager>();
        for (int i = 0; i < 6; i++)
        {
            DrawCard(hand);
        }
    }

    private void ShuffleDeck()
    {
        availableCards.Clear();
        availableCards.AddRange(allCards);

        for (int i = availableCards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Card temp = availableCards[i];
            availableCards[i] = availableCards[randomIndex];
            availableCards[randomIndex] = temp;
        }

        Debug.Log($"Колода перемешана. Карт в колоде: {availableCards.Count}");
    }

    public void DrawCard(HandManager handManager)
    {
        if (availableCards.Count == 0)
        {
            Debug.Log("Колода пуста! Новые карты не будут добавлены.");
            return;
        }

        Card nextCard = availableCards[0];
        availableCards.RemoveAt(0);
        handManager.AddCardToHand(nextCard);
        cardsDrawnCount++;

        Debug.Log($"Выдана карта: {nextCard.cardName}. Осталось карт: {availableCards.Count}");
    }

    // ВАЖНО: Новый метод для покупки карты
    public bool BuyCard(HandManager handManager)
    {
        if (availableCards.Count == 0)
        {
            Debug.Log("Колода пуста! Нельзя купить карту.");
            return false;
        }

        // Получаем PlayerStats для проверки золота
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats не найден!");
            return false;
        }

        // Рассчитываем стоимость карты: 10 + 5 за каждую 2 взятую карту
        int cardCost = CalculateCardCost();

        // Проверяем достаточно ли золота
        if (playerStats.currentGold >= cardCost)
        {
            // Списываем золото
            playerStats.SpendGold(cardCost);

            // Выдаем карту
            DrawCard(handManager);

            Debug.Log($"Куплена карта за {cardCost} золота. Всего взято карт: {cardsDrawnCount}");
            return true;
        }
        else
        {
            Debug.Log($"Недостаточно золота! Нужно: {cardCost}, есть: {playerStats.currentGold}");
            return false;
        }
    }

    // Расчет стоимости следующей карты
    public int CalculateCardCost()
    {
        // Базовая стоимость 10, +5 за каждую 2 взятую карту
        // cardsDrawnCount + 1 потому что считаем стоимость СЛЕДУЮЩЕЙ карты
        int cost = 2 + ((cardsDrawnCount + 1) / 2) * 5;
        return cost;
    }

    // Метод для удаления карты из доступных после использования
    public void RemoveCardFromAvailable(Card usedCard)
    {
        if (availableCards.Contains(usedCard))
        {
            availableCards.Remove(usedCard);
            Debug.Log($"Карта {usedCard.cardName} удалена из доступных. Осталось: {availableCards.Count}");
        }
    }

    // Метод для проверки, остались ли карты в колоде
    public bool HasCardsRemaining()
    {
        return availableCards.Count > 0;
    }

    // Метод для получения количества оставшихся карт
    public int GetRemainingCardsCount()
    {
        return availableCards.Count;
    }

    // Метод для получения стоимости следующей карты (для UI)
    public int GetNextCardCost()
    {
        return CalculateCardCost();
    }
}