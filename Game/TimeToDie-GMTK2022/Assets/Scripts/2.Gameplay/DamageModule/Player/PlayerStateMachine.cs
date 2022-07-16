using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : CharacterStateMachine
{
    [SerializeField] private PlayerStateBase currentState;
    public float Velocity;
    public Rigidbody RigidBody;
    public GameObject Indicator;

    public void SetState(PlayerStateBase state)
    {
        currentState = state;
        stateName = currentState.ToString();
    }

    private void Awake()
    {
        SetState(new PlayerDisableState(this));
        GameStateMachine.Singleton.OnGameStateChanged += HandleLevelStageChanged;
    }

    private void Update()
    {
        currentState.UpdateState();
        currentState.ProcessInput(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), Camera.main.ScreenToWorldPoint(Input.mousePosition));
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
        Jump,
        Ragdoll
    }
}
