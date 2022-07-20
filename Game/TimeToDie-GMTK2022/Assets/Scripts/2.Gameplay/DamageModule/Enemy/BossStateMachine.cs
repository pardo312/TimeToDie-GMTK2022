using Jiufen.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossStateMachine : MonoBehaviour, IDamageable
{
    public Action OnDead;
    public EnemyStatistics statistics;
    public List<Damage> damages = new List<Damage>();

    private BossStateBase currentState;
    public string currentStateName = "";


    public Animator animator;
    public Rigidbody rigidBody;
    public WeaponBase weapon;

    public List<Vector3> Positions;
    public List<Quaternion> Rotations;

    public int TargetTransform;
    public GameObject Model;

    public EnemyIdleState enemyIdleState;
    public EnemySeekState enemySeekState;
    public EnemyAttackState enemyAttackState;
    public EnemyHurstState enemyhurstState;
    public EnemyDeathState enemyDeathState;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        SetState(new BossDisableState(this));
        GameStateMachine.Singleton.OnGameStateChanged += HandleLevelStageChanged;
    }
    public void Start()
    {
        AudioManager.PlayAudio("BOSS", new AudioJobOptions() { loop = true });
    }

    private void Update()
    {
        currentState.UpdateState(Time.deltaTime);
        ApplyDamages();
    }

    public void SetState(BossStateBase state)
    {
        currentState = state;
    }

    private void HandleLevelStageChanged(LevelStage stage)
    {
        if (stage == LevelStage.gameMode)
        {
            SetState(new BossDisableState(this));
        }
        if (stage == LevelStage.inbetween)
        {
            SetState(new BossDisableState(this));
        }
        if (stage == LevelStage.victory)
        {
            SetState(new BossDisableState(this));
        }
    }

    public bool IsAtRange(Vector3 position)
    {
        return Vector3.Distance(position, transform.position) <= statistics.Range;
    }

    public bool IsPlayerAtRange()
    {
        return IsAtRange(GameStateMachine.Singleton.Player.transform.position);
    }

    public void ApplyDamages()
    {
        if (damages.Count <= 0)
            return;
        for (int i = 0; i < damages.Count; i++)
            damages[i].CalculateDamage();
        List<Damage> newDamages = new List<Damage>();
        for (int i = 0; i < damages.Count; i++)
            if (!damages[i].isOver)
                newDamages.Add(damages[i]);
        damages = newDamages;
    }

    [ContextMenu("AddCurrentPosRot")]
    public void AddCurrentPosRot()
    {
        Rotations.Add(transform.rotation);
        Positions.Add(transform.position);
    }

    [ContextMenu("Move by code")]
    public void MoveToTransform()
    {
        transform.position = Positions[TargetTransform];
        transform.rotation = Rotations[TargetTransform];
    }

    public void MoveToTransform(int target)
    {
        StartCoroutine(RoutineMovement(target));
    }

    IEnumerator RoutineMovement(int target)
    {
        Model.SetActive(false);
        TargetTransform = target;
        MoveToTransform();
        yield return null;
        Model.SetActive(true);
    }

    public virtual void AddDamage(Damage damageTaken)
    {
        damages.Add(damageTaken);
        damageTaken.target = this;
    }

    public virtual void TakeDamage(float amount)
    {
        statistics.Life -= amount;
        if (statistics.Life <= 0)
        {
            OnDead?.Invoke();
            return;
        }
    }
}
