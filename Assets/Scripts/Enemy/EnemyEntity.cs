using System;
using UnityEngine;

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemySO _enemySO;
    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;
    private int _currentHealth;

    private PolygonCollider2D _polygonCollider2D;
    private BoxCollider2D _boxCollider2D;
    private EnemyAI _enemyAI;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _enemyAI = GetComponent<EnemyAI>();
    }

    // —брасываем состо€ние при (ре)активации объекта Ч важно дл€ пулов и гарантии корректного состо€ни€ при Instantiate
    private void OnEnable()
    {
        if (_boxCollider2D != null) _boxCollider2D.enabled = true;
        if (_polygonCollider2D != null) _polygonCollider2D.enabled = true;

        if (_enemySO != null)
            _currentHealth = _enemySO.enemyHealth;
        else
            _currentHealth = 0;
    }

    private void Start()
    {
        // Start оставл€ем как есть Ч OnEnable уже инициализирует здоровье дл€ повторного использовани€
        if (_enemySO != null)
            _currentHealth = _enemySO.enemyHealth;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Player player))
        {
            player.TakeDamage(transform, _enemySO.enemyDamageAmount);
        }
    }
    public void PolygonColliderTurnOff()
    {
        if (_polygonCollider2D != null)
            _polygonCollider2D.enabled = false;
    }
    public void PolygonColliderTurnOn()
    {
        if (_polygonCollider2D != null)
            _polygonCollider2D.enabled = true;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }
    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            if (_boxCollider2D != null) _boxCollider2D.enabled = false;
            if (_polygonCollider2D != null) _polygonCollider2D.enabled = false;

            if (_enemyAI != null)
                _enemyAI.SetDeathState();

            // Ќачисл€ем золото игроку
            int goldToGive = (_enemySO != null) ? _enemySO.goldReward : 0;
            if (goldToGive > 0)
            {
                PlayerStats ps = null;
                if (Player.Instance != null)
                    ps = Player.Instance.GetComponent<PlayerStats>();

                if (ps == null)
                    ps = FindObjectOfType<PlayerStats>();

                if (ps != null)
                    ps.AddGold(goldToGive);
                else
                    Debug.LogWarning("[EnemyEntity] PlayerStats не найден Ч золото не начислено.");
            }

            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }
}