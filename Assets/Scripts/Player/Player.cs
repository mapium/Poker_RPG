using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnFlashBlink;
    public static Player Instance { get; private set; }
    [SerializeField] private float _movingSpeed = 10f;
    [SerializeField] private float _damageRecoveryTime = 0.5f;

    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;
    private bool _canTakeDamage;
    private bool _isAlive;

    private Rigidbody2D rb;
    private KnockBack _knockBack;
    private PolygonCollider2D _polygonCollider2D;

    // —сылка на PlayerStats
    [SerializeField] private PlayerStats playerStats;

    private void Start() {
        if (playerStats == null)
            playerStats = GetComponent<PlayerStats>();

        _canTakeDamage = true;
        _isAlive = true;
        GameInput.Instance._OnPlayerAttack += GameInput_OnPlayerAttack;
    }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _knockBack = GetComponent<KnockBack>();
        if (playerStats == null)
            playerStats = GetComponent<PlayerStats>();
    }

    void FixedUpdate()
    {
        if (_knockBack.IsGettingKnockBack) { return; }
        HandleMovement();
    }

    public bool IsRunning()
    {
        return _isRunning;
    }
    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }
    private void GameInput_OnPlayerAttack(object sender, EventArgs e)
    {
        Attacks.Instance.Attack();
    }

    public void TakeDamage(Transform damageSource, int damage)
    {
        if (_canTakeDamage && _isAlive)
        {
            _canTakeDamage = false;
            Debug.Log("aa");
            // »спользуем статы из PlayerStats
            playerStats.currentHealth = Mathf.Max(0, playerStats.currentHealth - damage);

            _knockBack.GetKnockedBack(damageSource);
            OnFlashBlink?.Invoke(this, EventArgs.Empty);
            StartCoroutine(DamageRecoveryRoutine());
        }
        DetectDeath();
    }

    private void DetectDeath()
    {
        if (playerStats.currentHealth == 0 && _isAlive)
        {
            _isAlive = false;
            _knockBack.StopKnockBackMovement();
            _canTakeDamage = false;
            GameInput.Instance.DisableMovement();
            OnPlayerDeath.Invoke(this, EventArgs.Empty);
        }
    }
    public bool IsAlive() => _isAlive;

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(_damageRecoveryTime);
        _canTakeDamage = true;
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        inputVector = inputVector.normalized;
        rb.MovePosition(rb.position + inputVector * (_movingSpeed * Time.fixedDeltaTime));
        if (Mathf.Abs(inputVector.x) > _minMovingSpeed || Mathf.Abs(inputVector.y) > _minMovingSpeed)
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }
    }
}