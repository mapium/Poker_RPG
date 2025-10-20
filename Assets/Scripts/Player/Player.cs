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
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private float _damageRecoveryTime = 0.5f;

    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;
    private int _currentHealth;
    private bool _canTakeDamage;
    private bool _isAlive;

    private Rigidbody2D rb;
    private KnockBack _knockBack;
    private PolygonCollider2D _polygonCollider2D;

    private void Start() {
        _currentHealth = _maxHealth;
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
            _currentHealth = Mathf.Max(0, _currentHealth -= damage);

            _knockBack.GetKnockedBack(damageSource);
            OnFlashBlink?.Invoke(this, EventArgs.Empty);
            StartCoroutine(DamageRecoveryRoutine());
        }
        DetectDeath();
    }

    private void DetectDeath()
    {
        if (_currentHealth == 0 && _isAlive)
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
