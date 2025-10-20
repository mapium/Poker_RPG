using System;
using UnityEngine;

public class FlashBlink : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _damagableObject;
    [SerializeField] private Material _blinkMaterial;
    [SerializeField] private float _blinkDuration = 0.2f;

    private float _blinkTimer;
    private Material _defaultMaterial;
    private SpriteRenderer _spriteRenderer;
    private bool _isBlinking;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultMaterial = GetComponent<Material>();

        _isBlinking = true;

        if (_damagableObject is Player)
        {
            (_damagableObject as Player).OnFlashBlink += _damagableObject_OnFlashBlink;
        }
    }

    private void _damagableObject_OnFlashBlink(object sender, EventArgs e)
    {
        SetBlinkingMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isBlinking)
        {
            _blinkTimer = Time.deltaTime;
            if (_blinkTimer < 0)
            {
                SetDefaultMaterial();
            }
        }
    }
    private void SetBlinkingMaterial()
    {
        _blinkTimer = _blinkDuration;
        _spriteRenderer.material = _blinkMaterial;
    }
    private void SetDefaultMaterial()
    {
        _spriteRenderer.material = _defaultMaterial;
    }
    public void StopBlinking()
    {
        SetDefaultMaterial();
        _isBlinking = false;
    }
}
