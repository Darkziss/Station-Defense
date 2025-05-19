using System;
using UnityEngine;
using StationDefense;
using Pooling;

[RequireComponent(typeof(BoxCollider2D), typeof(Health), typeof(EnemyAnimator))]
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private BoxCollider2D _boxCollider;

    [SerializeField] private Health _health;

    [SerializeField] private EnemyAnimator _enemyAnimator;

    [SerializeField] private int _ballLayer;
    [SerializeField] private int _baseLayer;
    [SerializeField] private int _circularWaveLayer;

    public abstract string EnemyName { get; }

    public ColorTeam Team { get; private set; }

    public static event Action<Vector2, int, ColorTeam> EnemyHit;

    protected virtual void OnValidate()
    {
        if (_transform == null)
            _transform = transform;
        
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_boxCollider == null)
            _boxCollider = GetComponent<BoxCollider2D>();

        if (_health == null)
            _health = GetComponent<Health>();

        if (_enemyAnimator == null)
            _enemyAnimator = GetComponent<EnemyAnimator>();
    }

    private void Start()
    {
        DeathHandler.GameRestarted += InstantDisable;
    }

    private void OnEnable()
    {
        _health.RestoreHealth();

        _boxCollider.enabled = true;

        _enemyAnimator.PlaySpawnAnimation();
    }

    public virtual void Init(ColorTeam team)
    {
        Team = team;

        _spriteRenderer.color = TeamColorStorage.GetByTeam(team);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int layer = collision.gameObject.layer;

        if (layer == _ballLayer)
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();

            bool isSameTeam = ball.Team == Team;

            int desiredDamage = isSameTeam ? ball.ColorDamage : ball.BaseDamage;

            _health.ChangeHealth(-desiredDamage);

            if (_health.IsHealthAtZero)
                DisableWithAnimation();
            else
                _enemyAnimator.PlayDamageAnimation();

            EnemyHit?.Invoke(_transform.position, desiredDamage, Team);
        }
        else if (layer == _circularWaveLayer)
        {
            CircularWave wave = collision.gameObject.GetComponent<CircularWave>();

            int desiredDamage = wave.Damage;

            _health.ChangeHealth(-desiredDamage);

            if (_health.IsHealthAtZero)
                DisableWithAnimation();

            EnemyHit?.Invoke(_transform.position, desiredDamage, ColorTeam.None);
        }
        else if (layer == _baseLayer)
        {
            InstantDisable();
        }
    }

    protected virtual void StopAction()
    {
        _boxCollider.enabled = false;
    }

    private void SetupOnEnable()
    {
        _boxCollider.enabled = true;

        if (!_health.IsHealthFull)
            _health.RestoreHealth();
    }

    private void InstantDisable()
    {
        StopAction();
        PutToPool();
    }

    private void DisableWithAnimation()
    {
        StopAction();

        _enemyAnimator.PlayDisableAnimation(PutToPool);
    }

    private void PutToPool() => PoolStorage.PutToPool(EnemyName, this);
}