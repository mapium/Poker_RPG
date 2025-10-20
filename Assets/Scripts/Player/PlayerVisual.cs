using System;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private const string IS_RUNNING = "isRunning";
    private const string IS_ATTACK = "Attack";
    private const string IS_DIE = "IsDie";
    private SpriteRenderer spriteRenderer;
    private FlashBlink _flashBlink;
    [SerializeField] private Attacks attacks;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _flashBlink = GetComponent<FlashBlink>();
    }

    private void Start()
    {
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
        attacks.OnAttack += Attacks_OnAttack;
    }

    private void Player_OnPlayerDeath(object sender, EventArgs e)
    {
        _flashBlink.StopBlinking();
        animator.SetBool(IS_DIE, true);
    }

    private void Update()
    {
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
        if (Player.Instance.IsAlive())
        {
            AdjustPlayerFacingDirection();
        }
        Attacks.Instance.RotateColliderToMouse();
    }

    private void Attacks_OnAttack(object sender, EventArgs e)
    {
        animator.SetTrigger(IS_ATTACK);
    }

    public void ResetAttackTrigger()
    {
        animator.ResetTrigger(IS_ATTACK);
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();

        if (mousePos.x < playerPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void TriggerEndAttackAnimation()
    {
        attacks.AttackColliderTurnOff();
    }
}
