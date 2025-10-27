using UnityEngine;

[CreateAssetMenu()]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public int enemyHealth;
    public int enemyDamageAmount;

    [Tooltip("������� ������ ��� ���� ���� ��� ������")]
    public int goldReward = 0;
}
