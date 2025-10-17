using System;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    public event EventHandler OnAttack;
    private PolygonCollider2D _PolygonCollider2D;
    public static Attacks Instance { get; private set; }
    [SerializeField] private int _damageAmount = 5;
    private void Awake()
    {
        _PolygonCollider2D = GetComponent<PolygonCollider2D>();
        Instance = this;
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
            enemyEntity.TakeDamage(_damageAmount);
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
