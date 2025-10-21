using System;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    public event EventHandler OnAttack;
    private PolygonCollider2D _PolygonCollider2D;
    public static Attacks Instance { get; private set; }

    [SerializeField] private int _damageAmount = 5;

    [SerializeField] private PlayerStats playerStats;

    private void Awake()
    {
        _PolygonCollider2D = GetComponent<PolygonCollider2D>();
        Instance = this;
        // не инициализируем playerStats здесь — сделаем это в Start, чтобы Player.Instance был готов
    }

    private void Start()
    {
        // Попробуем взять PlayerStats у Player.Instance, иначе падаем на FindObjectOfType как запасной вариант
        if (playerStats == null)
        {
            if (Player.Instance != null)
                playerStats = Player.Instance.GetComponent<PlayerStats>();

            if (playerStats == null)
                playerStats = FindObjectOfType<PlayerStats>();
        }

        AttackColliderTurnOff();
    }

    public void Attack()
    {
        AttackColliderTurnOffOn();
        OnAttack?.Invoke(this, EventArgs.Empty);
    }

    public void AttackColliderTurnOff()
    {
        _PolygonCollider2D.enabled = false;
    }
    private void AttackColliderTurnOn()
    {
        _PolygonCollider2D.enabled = true;
    }
    private void AttackColliderTurnOffOn()
    {
        AttackColliderTurnOff();
        AttackColliderTurnOn();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
        {
            Debug.Log("Hit Enemy");

            int damageToDeal = (playerStats != null) ? playerStats.currentDamage : _damageAmount;
            enemyEntity.TakeDamage(damageToDeal);
        }
    }
    public void RotateColliderToMouse()
    {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();

        if (mousePos.x < playerPosition.x)
            _PolygonCollider2D.transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            _PolygonCollider2D.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}