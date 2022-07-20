using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour, IDamageable
{
    public EnemyStatistics statistics;
    public List<Damage> damages = new List<Damage>();

    private EnemyStateBase currentState;
    public string currentStateName = "";

    public NavMeshAgent agent;
    public Animator animator;
    public Rigidbody rigidBody;
    public WeaponBase weapon;

    public int hashIdle, hashMovement, hashAttack, hashDeath, hashTakeDamage;
    public float durationIdle, durationMovement, durationAttack, durationDeath, durationTakeDamage;

    public EnemyIdleState enemyIdleState;
    public EnemySeekState enemySeekState;
    public EnemyAttackState enemyAttackState;
    public EnemyHurstState enemyhurstState;
    public EnemyDeathState enemyDeathState;

    private void Awake()
    {
        enemyIdleState = new EnemyIdleState(this, nameof(EnemyIdleState));
        enemySeekState = new EnemySeekState(this, nameof(EnemySeekState));
        enemyAttackState = new EnemyAttackState(this, nameof(EnemyAttackState));
        enemyhurstState = new EnemyHurstState(this, nameof(EnemyHurstState));
        enemyDeathState = new EnemyDeathState(this, nameof(EnemyDeathState));

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        weapon = GetComponentInChildren<WeaponBase>();

        GameStateMachine.Singleton.OnGameStateChanged += HandleLevelStageChanged;
    }

    void Start()
    {
        hashIdle = Animator.StringToHash("Idle");
        hashMovement = Animator.StringToHash("Movement");
        hashAttack = Animator.StringToHash("Attack");
        hashDeath = Animator.StringToHash("Death");
        hashTakeDamage = Animator.StringToHash("TakeDamage");

        SetState(enemyIdleState);
    }

    private void Update()
    {
        currentState.UpdateState(Time.deltaTime);
        ApplyDamages();
    }

    public void SetState(EnemyStateBase state)
    {
        if (currentState != null)
            currentState.ExitState();
        currentState = state;
        currentStateName = state.name;
        currentState.EnterState();
    }

    private void HandleLevelStageChanged(LevelStage stage)
    {
        if (stage == LevelStage.gameMode)
        {
            SetState(enemySeekState);
        }
        if (stage == LevelStage.inbetween)
        {
            SetState(enemyIdleState);
        }
        if (stage == LevelStage.victory)
        {
            SetState(enemyIdleState);
        }
        if (stage == LevelStage.loose)
        {
            SetState(enemyIdleState);
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
            SetState(enemyDeathState);
            return;
        }

        if(currentState != enemyhurstState)
            SetState(enemyhurstState);
        //TODO reduce amount to current life: stats.life -= amount;
        //Show UI damage effect
    }
}
