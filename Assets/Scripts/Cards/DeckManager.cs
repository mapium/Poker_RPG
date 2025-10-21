using UnityEngine;
using System.Collections.Generic;
using pokerpg;

public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();
    private List<Card> availableCards = new List<Card>();
    private int cardsDrawnCount = 0; // ������� ������ ����

    void Start()
    {
        // ��������� ��� ������ ���� �� ����� ��������
        Card[] cards = Resources.LoadAll<Card>("Cards");
        allCards.AddRange(cards);
        availableCards.AddRange(allCards);

        // ���������� ������
        ShuffleDeck();

        // ������� ��������� �����
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

        Debug.Log($"������ ����������. ���� � ������: {availableCards.Count}");
    }

    public void DrawCard(HandManager handManager)
    {
        if (availableCards.Count == 0)
        {
            Debug.Log("������ �����! ����� ����� �� ����� ���������.");
            return;
        }

        Card nextCard = availableCards[0];
        availableCards.RemoveAt(0);
        handManager.AddCardToHand(nextCard);
        cardsDrawnCount++;

        Debug.Log($"������ �����: {nextCard.cardName}. �������� ����: {availableCards.Count}");
    }

    // �����: ����� ����� ��� ������� �����
    public bool BuyCard(HandManager handManager)
    {
        if (availableCards.Count == 0)
        {
            Debug.Log("������ �����! ������ ������ �����.");
            return false;
        }

        // �������� PlayerStats ��� �������� ������
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats �� ������!");
            return false;
        }

        // ������������ ��������� �����: 10 + 5 �� ������ 2 ������ �����
        int cardCost = CalculateCardCost();

        // ��������� ���������� �� ������
        if (playerStats.currentGold >= cardCost)
        {
            // ��������� ������
            playerStats.SpendGold(cardCost);

            // ������ �����
            DrawCard(handManager);

            Debug.Log($"������� ����� �� {cardCost} ������. ����� ����� ����: {cardsDrawnCount}");
            return true;
        }
        else
        {
            Debug.Log($"������������ ������! �����: {cardCost}, ����: {playerStats.currentGold}");
            return false;
        }
    }

    // ������ ��������� ��������� �����
    public int CalculateCardCost()
    {
        // ������� ��������� 10, +5 �� ������ 2 ������ �����
        // cardsDrawnCount + 1 ������ ��� ������� ��������� ��������� �����
        int cost = 2 + ((cardsDrawnCount + 1) / 2) * 5;
        return cost;
    }

    // ����� ��� �������� ����� �� ��������� ����� �������������
    public void RemoveCardFromAvailable(Card usedCard)
    {
        if (availableCards.Contains(usedCard))
        {
            availableCards.Remove(usedCard);
            Debug.Log($"����� {usedCard.cardName} ������� �� ���������. ��������: {availableCards.Count}");
        }
    }

    // ����� ��� ��������, �������� �� ����� � ������
    public bool HasCardsRemaining()
    {
        return availableCards.Count > 0;
    }

    // ����� ��� ��������� ���������� ���������� ����
    public int GetRemainingCardsCount()
    {
        return availableCards.Count;
    }

    // ����� ��� ��������� ��������� ��������� ����� (��� UI)
    public int GetNextCardCost()
    {
        return CalculateCardCost();
    }
}