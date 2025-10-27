using TMPro;
using UnityEngine;
public class StatsVisual : MonoBehaviour
{
    [Header("�������� ������")]
    [Tooltip("���� �� ������ � ���������� ����� Player.Instance -> PlayerStats ��� FindObjectOfType<PlayerStats>()")]
    [SerializeField] private PlayerStats playerStats;

    [Header("UI �������� ��� �����������")]
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text damageText;

    // ������������ ��������, ����� ��������� ����� ������ ��� ���������
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

        // ��������� ������ ��� ��������� ��������
        if (playerStats.currentGold != _lastGold || playerStats.currentDamage != _lastDamage)
            Refresh();
    }

    // ��������� ����� ��� ��������������� ���������� (����� ������� �� ������ �������)
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