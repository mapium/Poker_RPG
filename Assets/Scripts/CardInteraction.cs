using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using pokerpg;

public class CardInteraction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Настройки взаимодействия")]
    public KeyCode useCardKey = KeyCode.E;
    public float doubleClickTime = 0.3f;

    private CardDisplay cardDisplay;
    private DragUIObject dragComponent;
    private bool isSelected = false;
    private float lastClickTime = 0f;
    private Image cardImage;

    void Start()
    {
        cardDisplay = GetComponent<CardDisplay>();
        dragComponent = GetComponent<DragUIObject>();
        cardImage = GetComponent<Image>();
    }

    void Update()
    {
        // Обработка нажатия E для применения карты
        if (isSelected && Input.GetKeyDown(useCardKey))
        {
            UseCardOnPlayer();
        }

        // Автоматическое применение при двойном клике
        if (isSelected && Time.time - lastClickTime < doubleClickTime && Input.GetMouseButtonDown(0))
        {
            UseCardOnPlayer();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isSelected = true;

        // Проверка на двойной клик
        if (Time.time - lastClickTime < doubleClickTime)
        {
            UseCardOnPlayer();
        }
        lastClickTime = Time.time;

        HighlightCard(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Можно добавить логику для сброса выделения при необходимости
    }

    private void UseCardOnPlayer()
    {
        if (cardDisplay == null || cardDisplay.cardData == null)
            return;

        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats != null)
        {
            // Сохраняем данные карты перед уничтожением
            Card usedCard = cardDisplay.cardData;

            playerStats.ApplyCardStats(usedCard);

            // Удаляем карту из доступных в DeckManager
            DeckManager deckManager = FindObjectOfType<DeckManager>();
            if (deckManager != null)
            {
                deckManager.RemoveCardFromAvailable(usedCard);
            }

            // Удаляем карту из руки
            HandManager handManager = FindObjectOfType<HandManager>();
            if (handManager != null && handManager.cardsInHand.Contains(gameObject))
            {
                handManager.RemoveCardFromHand(gameObject);
            }

            Destroy(gameObject);
            Debug.Log($"Карта {usedCard.cardName} применена к персонажу и удалена из доступных!");
        }
    }

    private void HighlightCard(bool highlight)
    {
        // Визуальное выделение карты
        if (cardImage != null)
        {
            cardImage.color = highlight ? new Color(1f, 1f, 0.7f, 1f) : Color.white;
        }
    }

    void OnDestroy()
    {
        // Снимаем выделение при уничтожении
        HighlightCard(false);
    }
}