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
    public TMP_Text nameText;
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
        nameText.text = cardData.cardName;
        healthText.text = cardData.health.ToString();
        dmgText.text = cardData.damage.ToString();
        defenseText.text = cardData.defense.ToString();
        goldText.text = cardData.gold.ToString();

        // �������� ��� ����������� ������
        foreach (var img in typeImage)
            img.gameObject.SetActive(false);

        // ���������� ����������� �����, ���� ���� ���� �� ���� �����
        if (cardData.cardType != null && cardData.cardType.Count > 0)
        {
            int suitIndex = (int)cardData.cardType[0]; // 0 - ������ �����
            if (suitIndex >= 0 && suitIndex < typeImage.Length)
                typeImage[suitIndex].gameObject.SetActive(true);
        }
    }
}
