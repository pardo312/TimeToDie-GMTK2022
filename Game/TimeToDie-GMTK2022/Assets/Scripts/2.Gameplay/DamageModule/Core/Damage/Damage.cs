using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Damage
{
    public IDamageable target;
    public DamageTypes damageType;
    public bool isOver;
    public float amount;

    public virtual void CalculateDamage()
    {
        Debug.Log("Damage");
        target.TakeDamage(amount);
        isOver = true;
    }
}

public enum DamageTypes{
    normal,
    electric,
    poison
}
