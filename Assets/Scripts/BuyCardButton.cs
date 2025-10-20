using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyCardButton : MonoBehaviour
{
    [Header("UI Elements")]
    public Button buyButton;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI cardsRemainingText;
    
    private DeckManager deckManager;
    private HandManager handManager;
    private PlayerStats playerStats;
    
    void Start()
    {
        deckManager = FindObjectOfType<DeckManager>();
        handManager = FindObjectOfType<HandManager>();
        playerStats = FindObjectOfType<PlayerStats>();
        
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(OnBuyCardClicked);
        }
        
        UpdateUI();
    }
    
    void Update()
    {
        UpdateUI();
    }
    
    private void OnBuyCardClicked()
    {
        if (deckManager != null && handManager != null)
        {
            bool success = deckManager.BuyCard(handManager);
            
            if (success)
            {
                // Можно добавить звук или анимацию успешной покупки
                Debug.Log("Карта успешно куплена!");
            }
            else
            {
                // Можно добавить звук или анимацию неудачной покупки
                Debug.Log("Не удалось купить карту!");
            }
            
            UpdateUI();
        }
    }
    
    private void UpdateUI()
    {
        if (deckManager != null)
        {
            // Обновляем стоимость
            if (costText != null)
            {
                int cost = deckManager.GetNextCardCost();
                costText.text = $"Купить карту: {cost} золота";
            }
            
            // Обновляем количество оставшихся карт
            if (cardsRemainingText != null)
            {
                int remaining = deckManager.GetRemainingCardsCount();
                cardsRemainingText.text = $"Карт в колоде: {remaining}";
            }
            
            // Блокируем кнопку если нет карт или недостаточно золота
            if (buyButton != null && playerStats != null)
            {
                int cost = deckManager.GetNextCardCost();
                bool canAfford = playerStats.currentGold >= cost;
                bool hasCards = deckManager.HasCardsRemaining();
                
                buyButton.interactable = canAfford && hasCards;
                
                // Меняем цвет кнопки в зависимости от доступности
                Image buttonImage = buyButton.GetComponent<Image>();
                if (buttonImage != null)
                {
                    if (!hasCards)
                        buttonImage.color = Color.gray;
                    else if (!canAfford)
                        buttonImage.color = Color.red;
                    else
                        buttonImage.color = Color.green;
                }
            }
        }
    }
}