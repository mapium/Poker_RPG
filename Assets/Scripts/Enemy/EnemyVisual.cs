using System;
using UnityEngine;

[RequireComponent (typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private EnemyEntity _enemyEntity;
    //[SerializeField] private GameObject _enemyShadow;
    private Animator _animator;
    private const string IS_RUNNING = "IsRunning";
    private const string TAKE_HIT = "TakeHit";
    private const string IS_DIE = "IsDie";
    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";
    private const string ATTACK = "Attack";

    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
        _enemyEntity.OnTakeHit += _enemyEntity_OnTakeHit;
        _enemyEntity.OnDeath += _enemyEntity_OnDeath;
    }

    private void _enemyEntity_OnDeath(object sender, EventArgs e)
    {
        _animator.SetBool(IS_DIE, true);
        _spriteRenderer.sortingOrder = -1;
        //_enemyShadow.SetActive(false);
    }

    private void _enemyEntity_OnTakeHit(object sender, EventArgs e)
    {
        _animator.SetTrigger(TAKE_HIT);
    }

    private void OnDestroy()
    {
        _enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
    }

    private void Update()
    {
        _animator.SetBool(IS_RUNNING, _enemyAI.IsAgentRunning());
        _animator.SetFloat(CHASING_SPEED_MULTIPLIER, _enemyAI.GetRoamingAnimationSpeed());
    }
    public void TriggerAttackAnimationTurnOff()
    {
        _enemyEntity.PolygonColliderTurnOff();
    }
    public void TriggerAttackAnimationTurnOn()
    {
        _enemyEntity.PolygonColliderTurnOn();
    }

    private void _enemyAI_OnEnemyAttack(object sender, EventArgs e)
    {
        _animator.SetTrigger(ATTACK);
    }
}
