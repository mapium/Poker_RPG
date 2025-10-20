using UnityEngine;
using pokerpg;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public class HandManager : MonoBehaviour
{
    public DeckManager deckManager;
    public GameObject cardPrefab; // префаб из юнити
    public Transform handTransform; // откуда рука идет
    public float fanSpread = 7.5f;

    public float cardSpacing = 100f;

    public float verticalSpacing = 100f;
    public List<GameObject> cardsInHand = new List<GameObject>(); // объекты карт в руке

    void Start()
    {
         
    }
    public void AddCardToHand(Card cardData)
    {
        //добавляет карту в руку
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardsInHand.Add(newCard);


        //внести данные карты для использующейся
        newCard.GetComponent<CardDisplay>().cardData = cardData;

        UpdateHandVisuals();
    }


    void Update()
    {
        //UpdateHandVisuals();

    }
    public void UpdateHandVisuals()
    {


        int cardCount = cardsInHand.Count;


        if (cardCount == 1)
        {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }
        for (int i = 0; i < cardCount; i++)
        {
            float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);


            float horizontalOffset = (cardSpacing * (i - (cardCount - 1) / 2f));

            float normalizedPosition = (2f * i / (cardCount - 1) - 1f); // чтоб ровными были межну 1 и -1
            float verticalOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition);


            // устанавливает позицию карт в руке
            cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
        }
    }

            public void RemoveCardFromHand(GameObject cardObject)
    {
        if (cardsInHand.Contains(cardObject))
        {
            cardsInHand.Remove(cardObject);
            UpdateHandVisuals();
        }
    }

    // Добавьте этот метод для очистки выделения всех карт
    public void DeselectAllCards()
    {
        foreach (GameObject card in cardsInHand)
        {
            CardInteraction interaction = card.GetComponent<CardInteraction>();
            if (interaction != null)
            {
                // Вызов метода снятия выделения через рефлексию
                card.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            }
        }
    }
}

