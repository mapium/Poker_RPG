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

    // ���������� ��� ����������� ������ (GameObject, ������ ������� ��������� healthText � �.�.)
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

        // �������� ��� ����������� ������
        foreach (var img in typeImage)
            img.gameObject.SetActive(false);

        // ���������� ����������� �����, ���� ���� ���� �� ���� �����
        int suitIndex = -1;
        if (cardData.cardType != null && cardData.cardType.Count > 0)
        {
            suitIndex = (int)cardData.cardType[0]; // 0 - ������ �����
            if (suitIndex >= 0 && suitIndex < typeImage.Length)
                typeImage[suitIndex].gameObject.SetActive(true);
        }

        // �������� ��� ������� ������ �� ���������
        if (healthObj != null) healthObj.SetActive(false);
        if (dmgObj != null) dmgObj.SetActive(false);
        if (defenseObj != null) defenseObj.SetActive(false);
        if (goldObj != null) goldObj.SetActive(false);

        // �������� ������ ������� � ����������� �� �����
        // ������: (����� �������� ��� ���� �������)
        // - Spades: ������ ����
        // - Hearts: ������ ��������
        // - Clubs: ������ ������
        // - Diamonds: ������ ������

        if (suitIndex == (int)Card.CardType.Spades && dmgObj != null)
            dmgObj.SetActive(true);
        else if (suitIndex == (int)Card.CardType.Hearts && healthObj != null)
            healthObj.SetActive(true);
        else if (suitIndex == (int)Card.CardType.Clubs && defenseObj != null)
            defenseObj.SetActive(true);
        else if (suitIndex == (int)Card.CardType.Diamonds && goldObj != null)
            goldObj.SetActive(true);

        // ���� ����� ���������� ��������� ������ ��� ����� ����� � �������� ������ ������ ����
    }
}
