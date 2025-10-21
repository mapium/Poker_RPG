using System;
using UnityEngine;
[RequireComponent(typeof(PlayerStats))]
public class Attacks : MonoBehaviour
{
    public event EventHandler OnAttack;
    private PolygonCollider2D _PolygonCollider2D;
    public static Attacks Instance { get; private set; }

    // Ѕазовый запас урона (fallback), можно оставить дл€ инспектора
    [SerializeField] private int _damageAmount = 5;

    // —сылка на PlayerStats Ч можно назначить в инспекторе или будет найдена автоматически
    [SerializeField] private PlayerStats playerStats;

    private void Awake()
    {
        _PolygonCollider2D = GetComponent<PolygonCollider2D>();
        Instance = this;

        {
            // ѕредпочтительно искать компонент на том же GameObject (обычно Attacks на игроке)
            playerStats = GetComponent<PlayerStats>();
        }
    }
    private void Start()
    {
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

            // »спользуем урон из PlayerStats если он доступен, иначе Ч fallback в _damageAmount
            int damageToDeal = (playerStats != null) ? playerStats.currentDamage : _damageAmount;
            enemyEntity.TakeDamage(damageToDeal);
        }
    }
    public void RotateColliderToMouse()
    {
        //if (_PolygonCollider2D == null) return;

        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();

        if (mousePos.x < playerPosition.x)
        {
            _PolygonCollider2D.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            _PolygonCollider2D.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }
}