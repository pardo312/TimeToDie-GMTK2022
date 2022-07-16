using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerStateBase currentState;
    public Animator animator;
    public string stateName;

    [Header("Player Stats")]
    public List<Damage> damages = new List<Damage>();

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
    }

    public virtual void TakeDamage(float amount)
    {
        //TODO reduce amount to current life: stats.life -= amount;
        //Show UI damage effect
    }
   
}
