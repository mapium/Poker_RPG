using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAI;
    private Animator _animator;
    private const string IS_RUNNING = "IsRunning";
    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool(IS_RUNNING, _enemyAI.IsAgentRunning());
        _animator.SetFloat(CHASING_SPEED_MULTIPLIER, _enemyAI.GetRoamingAnimationSpeed());
    }
}
