using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void AddDamage(Damage damageTaken);
    void TakeDamage(float amount);
}
