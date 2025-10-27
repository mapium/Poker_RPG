using UnityEngine;

[CreateAssetMenu()]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public int enemyHealth;
    public int enemyDamageAmount;

    [Tooltip("Сколько золота даёт этот враг при смерти")]
    public int goldReward = 0;
}
