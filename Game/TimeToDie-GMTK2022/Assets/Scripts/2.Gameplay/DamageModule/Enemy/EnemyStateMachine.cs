using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : CharacterStateMachine
{
    public Transform targetPosition, headPosition;
    public NavMeshPath path;
    public Rigidbody RigidBody;
    [Header("Animation")]
    public int hashVelocity;
    public int hashAttack, hashSpecialAttack, hashTurn;

    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
        hashVelocity = Animator.StringToHash("Velocity");
        hashAttack = Animator.StringToHash("Attack");
        hashSpecialAttack = Animator.StringToHash("SpecialAttack");
        hashTurn = Animator.StringToHash("Turn");
        SetState(new EnemySeekState(this));
    }

    [SerializeField] private EnemyStateBase currentState;

    public void SetState(EnemyStateBase state)
    {
        currentState = state;
        stateName = currentState.ToString();
    }

    private void Awake()
    {
        SetState(new EnemyDisableState(this));
    }

    private void Update()
    {
        currentState.UpdateState();
        //currentState.ProcessInput(GameStateMachine.Singleton.InputManager.Move, GameStateMachine.Singleton.InputManager.Look);
    }

    private void HandleLevelStageChanged(LevelStage stage)
    {
        if (stage == LevelStage.gameMode)
        {
            SetState(new EnemySeekState(this));
        }
        if (stage == LevelStage.inbetween)
        {
            SetState(new EnemyDisableState(this));
        }
        if (stage == LevelStage.victory)
        {
            SetState(new EnemyDisableState(this));
        }
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        //if (stats.life <= 0)
        //{
        //    //TODO manage dead state
        //}
    }

    public override void AddDamage(Damage damageTaken)
    {
        base.AddDamage(damageTaken);
        //animator.Play(getHitHash);
    }

    public enum Buttons
    {
        Aiming,
        Jump,
        Ragdoll
    }
}
