using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : CharacterStateMachine
{
    [SerializeField] private PlayerStateBase currentState;
    public Stats Stats;
    public Rigidbody RigidBody;
    public GameObject Indicator;
    public int currentWeapon;
    public List<WeaponBase> Weapons;
    public List<float> WeaponDuration;
    public List<int> attackHashes;
    [SerializeField] Transform weaponParent;

    public void SetState(PlayerStateBase state)
    {
        currentState = state;
        stateName = currentState.ToString();
    }

    private void Start()
    {
        attackHashes.Add(Animator.StringToHash("SwordAttack"));
        attackHashes.Add(Animator.StringToHash("SwordAttack"));
        attackHashes.Add(Animator.StringToHash("DrawArrow"));
    }

    private void Awake()
    {
        SetState(new PlayerDisableState(this));
        GameStateMachine.Singleton.OnGameStateChanged += HandleLevelStageChanged;
    }

    private void FixedUpdate()
    {
        RaycastHit Scenary;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out Scenary, 100, LayerMask.GetMask("Ground"));
        currentState.ProcessInput(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), Scenary.point);
    }

    private void Update()
    {
        currentState.UpdateState();
        if (Input.GetKeyDown(KeyCode.Mouse0))
            OnJoystickChange(Buttons.Attack);
    }

    private void OnJoystickChange(Buttons button)
    {
        currentState.ReceivedEvent(button);
    }


    private void HandleLevelStageChanged(LevelStage stage)
    {
        if (stage == LevelStage.gameMode)
        {
            SetState(new MovementState(this));
        }
        if (stage == LevelStage.inbetween)
        {
            SetState(new PlayerDisableState(this));
        }
        if (stage == LevelStage.victory)
        {
            SetState(new PlayerDisableState(this));
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
        Attack,
        Ragdoll
    }
}
