using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : CharacterStateMachine
{
    [SerializeField] private PlayerStateBase currentState;
    public AnimationBasedCharacterController AnimationBaseController;

    public void SetState(PlayerStateBase state)
    {
        currentState = state;
        stateName = currentState.ToString();
    }

    private void Awake()
    {
        SetState(new PlayerDisableState(this));
        GameStateMachine.Singleton.OnGameStateChanged += HandleLevelStageChanged;
        GameStateMachine.Singleton.InputManager.OnFireCallback += ()=>OnJoystickChange(Buttons.Aiming);
    }

    private void Start()
    {
        AnimationBaseController.Init();
    }

    private void Update()
    {
        currentState.UpdateState();
        currentState.ProcessInput(GameStateMachine.Singleton.InputManager.Move, GameStateMachine.Singleton.InputManager.Look);
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
