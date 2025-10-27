using UnityEngine;
using pokerpg;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;

    public Image cardImage;
    //public TMP_Text nameText;

    // Контейнеры для отображения статов (GameObject, внутри которых находятся healthText и т.д.)
    public GameObject healthObj;
    public GameObject dmgObj;
    public GameObject defenseObj;
    public GameObject goldObj;

    public TMP_Text healthText;
    public TMP_Text dmgText;
    public TMP_Text goldText;
    public TMP_Text defenseText;

    public Image[] typeImage;

    void Start()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        //nameText.text = cardData.cardName;
        healthText.text = cardData.health.ToString();
        dmgText.text = cardData.damage.ToString();
        defenseText.text = cardData.defense.ToString();
        goldText.text = cardData.gold.ToString();

        // Скрываем все изображения мастей
        foreach (var img in typeImage)
            img.gameObject.SetActive(false);

        // Отображаем изображение масти, если есть хотя бы одна масть
        int suitIndex = -1;
        if (cardData.cardType != null && cardData.cardType.Count > 0)
        {
            suitIndex = (int)cardData.cardType[0]; // 0 - первая масть
            if (suitIndex >= 0 && suitIndex < typeImage.Length)
                typeImage[suitIndex].gameObject.SetActive(true);
        }

        // Скрываем все объекты статов по умолчанию
        if (healthObj != null) healthObj.SetActive(false);
        if (dmgObj != null) dmgObj.SetActive(false);
        if (defenseObj != null) defenseObj.SetActive(false);
        if (goldObj != null) goldObj.SetActive(false);

        // Включаем нужные объекты в зависимости от масти
        // Пример: (можно изменить под ваши правила)
        // - Spades: только урон
        // - Hearts: только здоровье
        // - Clubs: только защита
        // - Diamonds: только золото

        if (suitIndex == (int)Card.CardType.Spades && dmgObj != null)
            dmgObj.SetActive(true);
        else if (suitIndex == (int)Card.CardType.Hearts && healthObj != null)
            healthObj.SetActive(true);
        else if (suitIndex == (int)Card.CardType.Clubs && defenseObj != null)
            defenseObj.SetActive(true);
        else if (suitIndex == (int)Card.CardType.Diamonds && goldObj != null)
            goldObj.SetActive(true);

        // Если нужно показывать несколько статов для одной карты — добавьте нужную логику ниже
    }
}
