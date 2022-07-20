using System;
using System.Collections;
using System.Collections.Generic;
using TimeToDie.DataManagerModule;
using UnityEngine;

public class PlayerStateMachine : CharacterStateMachine
{
    [SerializeField] private PlayerStateBase currentState;
    public Stats Stats;
    public Rigidbody RigidBody;
    public GameObject Indicator;
    [Header("Weapons")]
    public int currentWeapon;
    public List<WeaponBase> Weapons;
    public List<float> WeaponDuration;
    public List<int> attackHashes;
    [SerializeField] Transform weaponParent;
    [SerializeField] float timeBetweenChanges;
    [SerializeField] int takeDamageHash;
    float currentTimeBetweenChanges = 0.1f;

    public void SetState(PlayerStateBase state)
    {
        currentState = state;
        stateName = currentState.ToString();
    }

    private void Start()
    {
        takeDamageHash = Animator.StringToHash("DamageTaken");
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
        if (currentTimeBetweenChanges - Time.time < 0)
            SwapWeapons();
    }

    private void OnJoystickChange(Buttons button)
    {
        currentState.ReceivedEvent(button);
    }

    public void SwapWeapons()
    {
        Debug.Log("Swap Weapons!");
        currentTimeBetweenChanges = Time.time + timeBetweenChanges;
        int random = UnityEngine.Random.Range(0, Weapons.Count);
        currentWeapon = random;
        for (int i = 0; i < Weapons.Count; i++)
        {
            if (i == random)
            {
                Weapons[i].EnableVisual(true);
            }
            else
                Weapons[i].EnableVisual(false);
        }
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
            DataManager.instance.currentLevel++;
        }
        if (stage == LevelStage.loose)
        {
            SetState(new PlayerDisableState(this));
            DataManager.instance.currentLevel = 1;
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
        Stats.life -= damageTaken.amount;
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "idle2")
            animator.Play(takeDamageHash);
        if (Stats.life <= 0)
        {
            GameStateMachine.Singleton.SetLevelState(LevelStage.loose);

        }
        //animator.Play(getHitHash);
    }

    public enum Buttons
    {
        Aiming,
        Attack,
        Ragdoll
    }
}
